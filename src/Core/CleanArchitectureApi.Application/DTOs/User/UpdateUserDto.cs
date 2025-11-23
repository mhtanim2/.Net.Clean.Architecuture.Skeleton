using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureApi.Application.DTOs.User;

/// <summary>
/// Data Transfer Object for Update User
/// </summary>
public class UpdateUserDto
{
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    public bool? IsActive { get; set; }

    // TODO: Add additional properties as needed
    // public DateTime? DateOfBirth { get; set; }
    // public string? PhoneNumber { get; set; }
    // public string? ProfilePicture { get; set; }
    // public List<string>? Roles { get; set; }
}