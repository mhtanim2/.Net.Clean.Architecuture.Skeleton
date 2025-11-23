using MediatR;

namespace CleanArchitectureApi.Application.Features.Product.Queries.GetAllProducts;

/// <summary>
/// Query for retrieving all products
/// </summary>
public class GetAllProductsQuery : IRequest<List<ProductDto>>
{
    // TODO: Add query parameters as needed
    // public int PageNumber { get; set; } = 1;
    // public int PageSize { get; set; } = 10;
    // public string? SearchTerm { get; set; }
    // public bool? IsActive { get; set; }
}