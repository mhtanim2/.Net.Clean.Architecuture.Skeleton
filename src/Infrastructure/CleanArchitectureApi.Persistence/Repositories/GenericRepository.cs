using CleanArchitectureApi.Application.Contracts.Persistence;
using CleanArchitectureApi.Domain.Common;
using CleanArchitectureApi.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApi.Persistence.Repositories;

/// <summary>
/// Generic repository implementation for basic CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly CleanArchitectureDbContext _context;

    public GenericRepository(CleanArchitectureDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<IReadOnlyList<T>> GetAsync()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    // TODO: Add additional methods as needed
    // public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    // {
    //     return await _context.Set<T>().AsNoTracking().Where(predicate).ToListAsync();
    // }

    // public async Task<bool> ExistsAsync(int id)
    // {
    //     var entity = await _context.Set<T>().FindAsync(id);
    //     return entity != null;
    // }

    // public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
    // {
    //     if (predicate == null)
    //         return await _context.Set<T>().CountAsync();
    //     return await _context.Set<T>().CountAsync(predicate);
    // }
}