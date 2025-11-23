using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureApi.Identity.Models;

/// <summary>
/// Custom Application User
/// </summary>
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }

    // Navigation properties
    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; } = new List<IdentityUserRole<string>>();
    public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
    public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; } = new List<IdentityUserLogin<string>>();
    public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; } = new List<IdentityUserToken<string>>();

    // TODO: Add custom properties as needed
    // public string? PhoneNumberConfirmed { get; set; }
    // public string? TwoFactorEnabled { get; set; }
    // public string? RefreshToken { get; set; }
    // public DateTime? RefreshTokenExpiryTime { get; set; }

    // Computed properties
    public string FullName => $"{FirstName} {LastName}".Trim();

    // Methods
    public void UpdateLastLogin()
    {
        LastLoginAt = DateTime.UtcNow;
    }

    public bool IsEmailVerified => EmailConfirmed;
}