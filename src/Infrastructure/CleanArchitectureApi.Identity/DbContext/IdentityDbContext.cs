using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApi.Identity.DbContext;

/// <summary>
/// Identity database context
/// </summary>
public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure custom properties
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.ProfilePicture).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            // TODO: Add indexes as needed
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => new { e.FirstName, e.LastName });
            entity.HasIndex(e => e.IsActive);
        });

        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            // TODO: Add indexes as needed
            entity.HasIndex(e => e.Name).IsUnique();
            entity.HasIndex(e => e.IsActive);
        });

        // Seed initial data
        SeedInitialData(modelBuilder);
    }

    private void SeedInitialData(ModelBuilder modelBuilder)
    {
        // Seed roles
        var adminRole = new ApplicationRole
        {
            Id = "1",
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR",
            Description = "System administrator with full access",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var userRole = new ApplicationRole
        {
            Id = "2",
            Name = "User",
            NormalizedName = "USER",
            Description = "Standard user with limited access",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // TODO: Add more roles as needed
        var managerRole = new ApplicationRole
        {
            Id = "3",
            Name = "Manager",
            NormalizedName = "MANAGER",
            Description = "Manager with elevated access",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        modelBuilder.Entity<ApplicationRole>().HasData(adminRole, userRole, managerRole);

        // Seed admin user
        var adminUser = new ApplicationUser
        {
            Id = "1",
            UserName = "admin@localhost",
            NormalizedUserName = "ADMIN@LOCALHOST",
            Email = "admin@localhost",
            NormalizedEmail = "ADMIN@LOCALHOST",
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Administrator",
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Admin123!"),
            SecurityStamp = Guid.NewGuid().ToString()
        };

        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

        // Assign admin role to admin user
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = adminRole.Id,
                UserId = adminUser.Id
            });
    }
}