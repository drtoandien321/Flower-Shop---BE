using AnnFlowerProject.DTOs;

namespace AnnFlowerProject.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginDto);
        Task<LoginResponseDto?> SignUpAsync(SignUpRequestDto signUpDto);
        Task<bool> IsEmailExistsAsync(string email);
    }
}
