# Code Standards and Conventions

## 📝 C# Coding Standards

### 1. Naming Conventions

#### Classes and Interfaces
```csharp
// Classes - PascalCase
public class ProductService
public class OrderRepository
public class CreateOrderCommand

// Interfaces - PascalCase with I prefix
public interface IProductService
public interface IOrderRepository
public interface IEmailService

// Abstract classes - PascalCase
public abstract class BaseEntity
public abstract class ValidatorBase<T>
```

#### Methods and Properties
```csharp
// Methods - PascalCase
public async Task<List<ProductDto>> GetProductsAsync()
public void ValidateOrder(Order order)
private string GenerateToken()

// Properties - PascalCase
public int Id { get; set; }
public string Email { get; private set; }
public bool IsActive { get; init; }

// Constants - PascalCase
public const int MAX_RETRY_ATTEMPTS = 3;
public static readonly string DEFAULT_CONNECTION = "DefaultConnection";
```

#### Fields and Variables
```csharp
// Private fields - camelCase with _ prefix
private readonly IProductRepository _productRepository;
private readonly ILogger<ProductService> _logger;
private string _connectionString;

// Local variables - camelCase
var productList = await GetProducts();
var validationResult = new ValidationResult();
var pageNumber = 1;
```

#### Enums
```csharp
// Enums - PascalCase
public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}

// Enum with values
public enum UserRole
{
    Customer = 1,
    Manager = 2,
    Administrator = 3
}
```

### 2. File Organization

#### File Names
- Use PascalCase matching the class name
- One class per file (except nested classes)
- Keep related classes in same namespace

```
Features/
├── Product/
│   ├── Commands/
│   │   ├── CreateProduct/
│   │   │   ├── CreateProductCommand.cs
│   │   │   ├── CreateProductCommandHandler.cs
│   │   │   └── CreateProductCommandValidator.cs
│   │   └── UpdateProduct/
│   └── Queries/
│       └── GetProductById/
```

#### Using Statements
```csharp
// System namespaces first
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Microsoft namespaces
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// Third-party namespaces
using MediatR;
using AutoMapper;

// Project namespaces
using YourProject.Domain;
using YourProject.Application.DTOs;
```

### 3. Code Structure

#### Class Structure
```csharp
// 1. Using statements
using System;

// 2. Namespace
namespace YourProject.Application.Services;

// 3. Class documentation
/// <summary>
/// Service for managing products
/// </summary>
public class ProductService : IProductService
{
    // 4. Private fields
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;

    // 5. Constructor
    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // 6. Public methods
    public async Task<ProductDto> GetProductAsync(int id)
    {
        // Implementation
    }

    // 7. Private methods
    private ProductDto MapToDto(Product product)
    {
        // Implementation
    }
}
```

### 4. Method Implementation Standards

#### Method Signature
```csharp
// Always use async/await for async operations
public async Task<List<ProductDto>> GetProductsAsync(GetProductsQuery query)
{
    // Implementation
}

// Use CancellationToken for async methods
public async Task<ProductDto> GetProductAsync(int id, CancellationToken cancellationToken = default)
{
    // Implementation
}
```

#### Method Body
```csharp
public async Task<ProductDto> CreateProductAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
{
    // 1. Validate input
    if (command == null)
        throw new ArgumentNullException(nameof(command));

    // 2. Log operation
    _logger.LogInformation("Creating product: {ProductName}", command.Name);

    try
    {
        // 3. Business logic
        var product = _mapper.Map<Product>(command);
        product.CreatedAt = DateTime.UtcNow;

        // 4. Persist
        await _productRepository.CreateAsync(product);
        await _productRepository.SaveChangesAsync(cancellationToken);

        // 5. Log success
        _logger.LogInformation("Product created successfully with ID: {ProductId}", product.Id);

        // 6. Return result
        return _mapper.Map<ProductDto>(product);
    }
    catch (Exception ex)
    {
        // 7. Log error
        _logger.LogError(ex, "Error creating product: {ProductName}", command.Name);
        throw;
    }
}
```

### 5. Error Handling Standards

#### Exception Types
```csharp
// Use specific exception types
throw new NotFoundException($"Product with ID {id} not found");
throw new BadRequestException("Invalid product data", validationResult);
throw new UnauthorizedAccessException("User not authorized");
throw new InvalidOperationException("Product cannot be deleted");

// Never throw generic Exception
throw new Exception("Something went wrong"); // AVOID
```

#### Null Checking
```csharp
// Method arguments
public ProductService(IProductRepository productRepository)
{
    _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
}

// Method parameters
public async Task UpdateProduct(int id, UpdateProductCommand command)
{
    if (command == null)
        throw new ArgumentNullException(nameof(command));

    if (id <= 0)
        throw new ArgumentException("ID must be greater than 0", nameof(id));
}

// Null propagation and coalescing
var productName = product?.Name ?? "Unknown";
var result = data?.FirstOrDefault() ?? defaultResult;
```

### 6. LINQ Standards

#### Method Syntax (Preferred)
```csharp
// Use method syntax for complex queries
var products = await _context.Products
    .Where(p => p.IsActive && p.StockQuantity > 0)
    .OrderBy(p => p.Name)
    .Select(p => new ProductDto
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price
    })
    .ToListAsync();
```

#### Query Syntax (When appropriate)
```csharp
// Use query syntax for joins and complex set operations
var query = from product in _context.Products
           join category in _context.Categories on product.CategoryId equals category.Id
           where product.IsActive
           select new ProductDto
           {
               Id = product.Id,
               Name = product.Name,
               CategoryName = category.Name
           };
```

### 7. Async/Await Standards

#### Always await async calls
```csharp
// Correct
var products = await GetProductsAsync();
await _repository.SaveAsync();

// Incorrect - don't do this
var task = GetProductsAsync();
task.Wait();
var products = task.Result;
```

#### Configure await
```csharp
// Use ConfigureAwait(false) in library code
var result = await SomeAsyncMethod().ConfigureAwait(false);

// Not necessary in application code (ASP.NET Core)
var result = await SomeAsyncMethod();
```

### 8. String Manipulation

#### Use string interpolation
```csharp
// Preferred
var message = $"Product {product.Name} (ID: {product.Id}) created successfully";

// Avoid string concatenation
var message = "Product " + product.Name + " (ID: " + product.Id + ") created successfully";
```

#### StringBuilder for multiple concatenations
```csharp
var sb = new StringBuilder();
sb.AppendLine("Product Report");
sb.AppendLine($"Total Products: {total}");
sb.AppendLine($"Active Products: {active}");
return sb.ToString();
```

### 9. Collection Standards

#### Use collection initializers
```csharp
// Preferred
var products = new List<Product>
{
    new Product { Name = "Product 1", Price = 10.99m },
    new Product { Name = "Product 2", Price = 19.99m }
};

// Verbose way
var products = new List<Product>();
products.Add(new Product { Name = "Product 1", Price = 10.99m });
products.Add(new Product { Name = "Product 2", Price = 19.99m });
```

#### Use IReadOnlyList for returns
```csharp
// Method signature
public async Task<IReadOnlyList<ProductDto>> GetProductsAsync()

// Implementation
return products.AsReadOnly();
```

### 10. Comments and Documentation

#### XML Documentation
```csharp
/// <summary>
/// Creates a new product
/// </summary>
/// <param name="command">The product creation command</param>
/// <param name="cancellationToken">Cancellation token</param>
/// <returns>The ID of the created product</returns>
/// <exception cref="ArgumentNullException">Thrown when command is null</exception>
/// <exception cref="ValidationException">Thrown when validation fails</exception>
public async Task<int> CreateProductAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
{
    // Implementation
}
```

#### Inline Comments
```csharp
// Calculate discount based on customer tier
var discount = customer.Tier switch
{
    CustomerTier.Gold => 0.20m,
    CustomerTier.Silver => 0.10m,
    _ => 0m
};

// Apply business rule: minimum order value is $10
if (order.Total < 10.00m)
{
    throw new InvalidOperationException("Order total must be at least $10.00");
}
```

### 11. Constants vs Magic Numbers

```csharp
// Define constants
public const int MAX_RETRY_ATTEMPTS = 3;
public const decimal MIN_ORDER_VALUE = 10.00m;
public static readonly TimeSpan LOCK_TIMEOUT = TimeSpan.FromMinutes(5);

// Use instead of magic numbers
if (retryCount < MAX_RETRY_ATTEMPTS)
{
    // Retry logic
}

if (order.Total < MIN_ORDER_VALUE)
{
    // Handle minimum value
}
```

### 12. Expression Body Members

Use for simple properties and methods:
```csharp
// Simple properties
public int ProductCount => _products.Count;
public bool IsEmpty => !_products.Any();
public string DisplayName => $"{FirstName} {LastName}".Trim();

// Simple methods
public string GetStatus() => IsActive ? "Active" : "Inactive";
public decimal CalculateTax() => Total * TaxRate;
```

## 🎯 Key Rules Summary

1. **Follow C# naming conventions** strictly
2. **Keep methods small** and focused
3. **Always validate input** at the start
4. **Handle exceptions properly** with specific types
5. **Use async/await correctly**
6. **Document public APIs** with XML comments
7. **Avoid magic numbers/strings** - use constants
8. **Keep code readable** and maintainable
9. **Follow SOLID principles**
10. **Write code for humans first**, computers second