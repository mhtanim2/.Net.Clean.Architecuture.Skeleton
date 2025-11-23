using CleanArchitectureApi.Domain.Common;

namespace CleanArchitectureApi.Application.Contracts.Persistence;

/// <summary>
/// Generic repository interface for basic CRUD operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAsync();
    Task<T> GetByIdAsync(int id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);

    // TODO: Add additional methods as needed
    // Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
    // Task<T> GetEntityWithSpec(ISpecification<T> spec);
    // Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    // Task<int> CountAsync(ISpecification<T> spec);
    // void AddEntity(T entity);
    // void UpdateEntity(T entity);
    // void DeleteEntity(T entity);
}