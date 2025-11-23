using CleanArchitectureApi.Domain.Entities;

namespace CleanArchitectureApi.Application.Contracts.Persistence;

/// <summary>
/// Specific repository for Product entity with custom methods
/// </summary>
public interface IProductRepository : IGenericRepository<Product>
{
    // TODO: Add product-specific repository methods here
    // Task<Product?> GetBySKUAsync(string sku);
    // Task<IReadOnlyList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    // Task<IReadOnlyList<Product>> GetActiveProductsAsync();
    // Task<bool> IsSKUUniqueAsync(string sku, int? excludeId = null);
}