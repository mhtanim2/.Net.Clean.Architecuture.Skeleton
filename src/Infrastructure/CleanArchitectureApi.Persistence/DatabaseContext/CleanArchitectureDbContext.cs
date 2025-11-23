using CleanArchitectureApi.Application.Contracts.Persistence;
using CleanArchitectureApi.Domain.Common;
using CleanArchitectureApi.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;

namespace CleanArchitectureApi.Persistence.DatabaseContext;

/// <summary>
/// Entity Framework database context
/// </summary>
public class CleanArchitectureDbContext : DbContext
{
    private readonly IHttpContextAccessor? _httpContextAccessor;

    public CleanArchitectureDbContext(DbContextOptions<CleanArchitectureDbContext> options)
        : base(options)
    {
    }

    public CleanArchitectureDbContext(DbContextOptions<CleanArchitectureDbContext> options,
        IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    // DbSets for entities
    public DbSet<Product> Products { get; set; }

    // TODO: Add DbSets for other entities
    // public DbSet<Order> Orders { get; set; }
    // public DbSet<Customer> Customers { get; set; }
    // public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // TODO: Add custom model configurations
        // modelBuilder.Entity<Order>()
        //     .HasOne(o => o.Customer)
        //     .WithMany(c => c.Orders)
        //     .HasForeignKey(o => o.CustomerId);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure sensitive data logging for development
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        // TODO: Add database configuration
        // optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

        base.OnConfiguring(optionsBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Set audit fields
        foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    entry.Entity.CreatedBy = GetCurrentUserId();
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.DateCreated).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;
                    entry.Entity.DateModified = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = GetCurrentUserId();
                    break;

                case EntityState.Detached:
                    break;

                case EntityState.Unchanged:
                    break;

                case EntityState.Deleted:
                    entry.Entity.DateModified = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = GetCurrentUserId();
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private string? GetCurrentUserId()
    {
        // TODO: Get user ID based on your authentication setup
        // This example assumes HttpContext is available and has user claims
        if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
        {
            return _httpContextAccessor.HttpContext.User?.FindFirst("sub")?.Value ??
                   _httpContextAccessor.HttpContext.User?.FindFirst("userId")?.Value ??
                   _httpContextAccessor.HttpContext.User?.FindFirst("nameid")?.Value;
        }

        return "System";
    }

    public override int SaveChanges()
    {
        // Set audit fields
        foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    entry.Entity.CreatedBy = GetCurrentUserId();
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.DateCreated).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;
                    entry.Entity.DateModified = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = GetCurrentUserId();
                    break;

                case EntityState.Detached:
                    break;

                case EntityState.Unchanged:
                    break;

                case EntityState.Deleted:
                    entry.Entity.DateModified = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = GetCurrentUserId();
                    break;
            }
        }

        return base.SaveChanges();
    }
}