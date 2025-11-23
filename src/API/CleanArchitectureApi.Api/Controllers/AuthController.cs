using CleanArchitectureApi.Application.Contracts.Identity;
using CleanArchitectureApi.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureApi.Api.Controllers;

/// <summary>
/// Controller for authentication operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// User login
    /// </summary>
    /// <param name="loginDto">Login credentials</param>
    /// <returns>Authentication response</returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        var authResponse = await _authService.LoginAsync(loginDto);

        if (authResponse == null)
            return Unauthorized("Invalid credentials");

        return Ok(authResponse);
    }

    /// <summary>
    /// User registration
    /// </summary>
    /// <param name="registerDto">Registration data</param>
    /// <returns>Authentication response</returns>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
    {
        var authResponse = await _authService.RegisterAsync(registerDto);

        if (authResponse == null)
            return BadRequest("Registration failed. User may already exist.");

        return Ok(authResponse);
    }

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="changePasswordDto">Password change data</param>
    /// <returns>Success status</returns>
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User not authenticated");

        var result = await _authService.ChangePasswordAsync(userId, changePasswordDto);

        if (!result)
            return BadRequest("Password change failed");

        return Ok("Password changed successfully");
    }

    /// <summary>
    /// Reset password
    /// </summary>
    /// <param name="resetPasswordDto">Password reset data</param>
    /// <returns>Success status</returns>
    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var result = await _authService.ResetPasswordAsync(resetPasswordDto);

        if (!result)
            return BadRequest("Password reset failed");

        return Ok("Password reset successful");
    }

    /// <summary>
    /// User logout
    /// </summary>
    /// <returns>Success status</returns>
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User not authenticated");

        await _authService.LogoutAsync(userId);

        return Ok("Logout successful");
    }

    /// <summary>
    /// Get current user information
    /// </summary>
    /// <returns>User details</returns>
    [HttpGet("me")]
    public async Task<ActionResult> GetCurrentUser()
    {
        var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User not authenticated");

        var user = await _authService.GetUserByIdAsync(userId);

        if (user == null)
            return NotFound("User not found");

        return Ok(user);
    }

    // TODO: Add additional endpoints as needed
    // [HttpPost("forgot-password")]
    // public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    // {
    //     var token = await _authService.GeneratePasswordResetTokenAsync(forgotPasswordDto.Email);
    //
    //     if (string.IsNullOrEmpty(token))
    //         return BadRequest("Email not found");
    //
    //     // TODO: Send email with reset token
    //     // await _emailService.SendPasswordResetEmailAsync(forgotPasswordDto.Email, token);
    //
    //     return Ok("Password reset link sent to email");
    // }
    //
    // [HttpPost("verify-email")]
    // public async Task<ActionResult> VerifyEmail([FromBody] VerifyEmailDto verifyEmailDto)
    // {
    //     var result = await _authService.VerifyEmailAsync(verifyEmailDto.UserId, verifyEmailDto.Token);
    //
    //     if (!result)
    //         return BadRequest("Email verification failed");
    //
    //     return Ok("Email verified successfully");
    // }
}