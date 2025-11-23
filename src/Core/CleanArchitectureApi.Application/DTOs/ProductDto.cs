namespace CleanArchitectureApi.Application.DTOs;

/// <summary>
/// Data Transfer Object for Product
/// </summary>
public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string? SKU { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? DateCreated { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public string? ModifiedBy { get; set; }

    // TODO: Add computed properties as needed
    // public bool IsInStock => StockQuantity > 0 && IsActive;
    // public string StockStatus => IsInStock ? "In Stock" : "Out of Stock";
}