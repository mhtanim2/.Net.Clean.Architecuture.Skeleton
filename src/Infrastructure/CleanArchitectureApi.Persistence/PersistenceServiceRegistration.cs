using CleanArchitectureApi.Application.Contracts.Persistence;
using CleanArchitectureApi.Persistence.DatabaseContext;
using CleanArchitectureApi.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureApi.Persistence;

/// <summary>
/// Service registration for Persistence layer
/// </summary>
public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<CleanArchitectureDbContext>(options =>
        {
            // TODO: Configure your database provider
            // For SQL Server:
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            // For PostgreSQL:
            // options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            // For MySQL:
            // options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
            //     ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));

            // For SQLite:
            // options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        // Register HttpContextAccessor for audit fields
        services.AddHttpContextAccessor();

        // Register repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IProductRepository, ProductRepository>();

        // TODO: Register other repositories as needed
        // services.AddScoped<IOrderRepository, OrderRepository>();
        // services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}