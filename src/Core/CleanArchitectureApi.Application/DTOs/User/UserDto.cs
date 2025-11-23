namespace CleanArchitectureApi.Application.DTOs.User;

/// <summary>
/// Data Transfer Object for User
/// </summary>
public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; }

    // TODO: Add additional properties as needed
    // public DateTime? DateOfBirth { get; set; }
    // public string? PhoneNumber { get; set; }
    // public string? ProfilePicture { get; set; }
    // public bool EmailConfirmed { get; set; }
    // public bool TwoFactorEnabled { get; set; }
}