using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureApi.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for Change Password
/// </summary>
public class ChangePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}