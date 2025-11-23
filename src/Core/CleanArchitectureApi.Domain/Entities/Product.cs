using CleanArchitectureApi.Domain.Common;

namespace CleanArchitectureApi.Domain.Entities;

/// <summary>
/// Sample Product entity - Replace with your actual domain entities
/// This is a placeholder showing the pattern for domain entities
/// </summary>
public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? SKU { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime? CreatedAt { get; set; }

    // TODO: Add navigation properties as needed
    // public int CategoryId { get; set; }
    // public Category Category { get; set; }

    // TODO: Add domain methods and business logic
    // public void UpdateStock(int quantity)
    // {
    //     if (quantity < 0)
    //         throw new ArgumentException("Stock cannot be negative");
    //
    //     StockQuantity = quantity;
    // }

    // public bool IsInStock()
    // {
    //     return StockQuantity > 0 && IsActive;
    // }
}