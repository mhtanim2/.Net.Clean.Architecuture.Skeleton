using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureApi.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for Registration
/// </summary>
public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    // TODO: Add additional fields as needed
    // public DateTime? DateOfBirth { get; set; }
    // public string? PhoneNumber { get; set; }
    // public string? ProfilePicture { get; set; }
}