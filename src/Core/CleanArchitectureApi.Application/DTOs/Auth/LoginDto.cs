using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureApi.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for Login
/// </summary>
public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}