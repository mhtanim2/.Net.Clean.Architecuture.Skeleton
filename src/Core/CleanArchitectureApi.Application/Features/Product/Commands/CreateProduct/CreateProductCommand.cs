using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitectureApi.Application.Features.Product.Commands.CreateProduct;

/// <summary>
/// Command for creating a new product
/// </summary>
public class CreateProductCommand : IRequest<int>
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [MaxLength(50)]
    public string? SKU { get; set; }

    public bool IsActive { get; set; } = true;
}