using CleanArchitectureApi.Application.DTOs;

namespace CleanArchitectureApi.Application.Contracts.Identity;

/// <summary>
/// Service interface for authentication operations
/// </summary>
public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
    Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<bool> LogoutAsync(string userId);
    Task<UserDto?> GetUserByIdAsync(string userId);

    // TODO: Add additional method signatures as needed
    // Task<bool> VerifyEmailAsync(string userId, string token);
    // Task<string> GeneratePasswordResetTokenAsync(string email);
    // Task<bool> ConfirmEmailAsync(string userId, string token);
    // Task<RefreshTokenDto> RefreshTokenAsync(string token);
}