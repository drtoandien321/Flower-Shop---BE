using AnnFlowerProject.DTOs;
using AnnFlowerProject.Models;
using AnnFlowerProject.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AnnFlowerProject.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto)
        {
            // Tìm user theo email
            var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return null;
            }

            // Verify password
            if (!VerifyPassword(loginDto.Password, user.Password))
            {
                return null;
            }

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return new LoginResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    UserId = user.UserId,
                    Fullname = user.Fullname,
                    Email = user.Email,
                    Phone = user.Phone,
                    RoleName = user.Role?.RoleName ?? string.Empty
                }
            };
        }

        public async Task<LoginResponseDto?> SignUpAsync(SignUpRequestDto signUpDto)
        {
            // Kiểm tra email đã tồn tại chưa
            if (await IsEmailExistsAsync(signUpDto.Email))
            {
                return null;
            }

            // Hash password
            var hashedPassword = HashPassword(signUpDto.Password);

            // Tạo user mới với role Customer (RoleId = 2)
            var newUser = new User
            {
                Fullname = signUpDto.Fullname,
                Email = signUpDto.Email,
                Password = hashedPassword,
                Phone = signUpDto.Phone,
                RoleId = 2 // Customer role
            };

            await _unitOfWork.Users.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();

            // Load role information
            var userWithRole = await _unitOfWork.Users.GetByIdWithRoleAsync(newUser.UserId);

            if (userWithRole == null)
                return null;

            // Generate JWT token
            var token = GenerateJwtToken(userWithRole);

            return new LoginResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    UserId = userWithRole.UserId,
                    Fullname = userWithRole.Fullname,
                    Email = userWithRole.Email,
                    Phone = userWithRole.Phone,
                    RoleName = userWithRole.Role?.RoleName ?? string.Empty
                }
            };
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _unitOfWork.Users.IsEmailExistsAsync(email);
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.RoleName ?? "Customer"),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
            catch
            {
                // Fallback cho trường hợp password chưa hash
                return password == hashedPassword;
            }
        }
    }
}
