using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AnnFlowerProject.DTOs;
using System.ComponentModel;
using AnnFlowerProject.Services.Interfaces;

namespace AnnFlowerProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        [HttpPost("login")]
        [Description("Đăng nhập")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(loginDto);

            if (result == null)
            {
                return Unauthorized(new { message = "Email hoặc mật khẩu không đúng" });
            }

            return Ok(result);
        }

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        [HttpPost("signup")]
        [Description("Đăng ký tài khoản mới")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDto signUpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Kiểm tra email đã tồn tại
            if (await _authService.IsEmailExistsAsync(signUpDto.Email))
            {
                return BadRequest(new { message = "Email đã được sử dụng" });
            }

            var result = await _authService.SignUpAsync(signUpDto);

            if (result == null)
            {
                return BadRequest(new { message = "Đăng ký thất bại" });
            }

            return Ok(result);
        }

        /// <summary>
        /// Kiểm tra email đã tồn tại chưa
        /// </summary>
        [HttpGet("check-email")]
        [Description("Kiểm tra email đã tồn tại chưa")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckEmail([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { message = "Email không được để trống" });
            }

            var exists = await _authService.IsEmailExistsAsync(email);
            return Ok(new { exists });
        }
    }
}
