using Microsoft.AspNetCore.Identity;

namespace CleanArchitectureApi.Identity.Models;

/// <summary>
/// Custom Application Role
/// </summary>
public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<IdentityRoleClaim<string>> Claims { get; set; } = new List<IdentityRoleClaim<string>>();
    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();

    // TODO: Add custom properties as needed
    // public int RoleHierarchy { get; set; }
    // public string? Department { get; set; }
    // public bool IsSystemRole { get; set; }
}