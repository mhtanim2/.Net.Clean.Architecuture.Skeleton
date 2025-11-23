using CleanArchitectureApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureApi.Persistence.Configurations;

/// <summary>
/// Entity Framework configuration for Product entity
/// </summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Table configuration
        builder.ToTable("Products");

        // Primary key
        builder.HasKey(p => p.Id);

        // Property configurations
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.SKU)
            .HasMaxLength(50);

        // Index configurations
        builder.HasIndex(p => p.SKU)
            .IsUnique()
            .HasDatabaseName("IX_Products_SKU");

        builder.HasIndex(p => p.Name)
            .HasDatabaseName("IX_Products_Name");

        builder.HasIndex(p => p.IsActive)
            .HasDatabaseName("IX_Products_IsActive");

        // TODO: Add relationships when available
        // builder.HasOne(p => p.Category)
        //     .WithMany(c => c.Products)
        //     .HasForeignKey(p => p.CategoryId)
        //     .OnDelete(DeleteBehavior.Restrict);

        // Seed data for development
        builder.HasData(
            new Product
            {
                Id = 1,
                Name = "Sample Product 1",
                Description = "This is a sample product for testing",
                Price = 99.99m,
                StockQuantity = 100,
                SKU = "SAMPLE-001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                DateCreated = DateTime.UtcNow,
                CreatedBy = "System"
            },
            new Product
            {
                Id = 2,
                Name = "Sample Product 2",
                Description = "Another sample product for testing",
                Price = 149.99m,
                StockQuantity = 50,
                SKU = "SAMPLE-002",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                DateCreated = DateTime.UtcNow,
                CreatedBy = "System"
            });

        // TODO: Configure query filters for soft deletes if needed
        // builder.HasQueryFilter(p => !p.IsDeleted);

        // TODO: Configure value conversions
        // builder.Property(p => p.Status)
        //     .HasConversion<string>();
    }
}