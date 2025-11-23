using CleanArchitectureApi.Application.Contracts.Persistence;
using CleanArchitectureApi.Domain.Entities;
using CleanArchitectureApi.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApi.Persistence.Repositories;

/// <summary>
/// Specific repository implementation for Product entity
/// </summary>
public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(CleanArchitectureDbContext context) : base(context)
    {
    }

    // TODO: Implement product-specific methods
    // public async Task<Product?> GetBySKUAsync(string sku)
    // {
    //     return await _context.Products
    //         .AsNoTracking()
    //         .FirstOrDefaultAsync(p => p.SKU == sku);
    // }

    // public async Task<IReadOnlyList<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    // {
    //     return await _context.Products
    //         .AsNoTracking()
    //         .Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.IsActive)
    //         .ToListAsync();
    // }

    // public async Task<IReadOnlyList<Product>> GetActiveProductsAsync()
    // {
    //     return await _context.Products
    //         .AsNoTracking()
    //         .Where(p => p.IsActive && p.StockQuantity > 0)
    //         .ToListAsync();
    // }

    // public async Task<bool> IsSKUUniqueAsync(string sku, int? excludeId = null)
    // {
    //     var query = _context.Products.AsNoTracking().Where(p => p.SKU == sku);
    //
    //     if (excludeId.HasValue)
    //         query = query.Where(p => p.Id != excludeId.Value);
    //
    //     return !await query.AnyAsync();
    // }
}