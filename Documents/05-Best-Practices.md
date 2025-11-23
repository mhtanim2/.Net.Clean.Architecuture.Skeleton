# Best Practices and Rules for Clean Architecture API Development

## ✅ DO's - Always Follow These Practices

### Architecture DO's

1. **DO follow the Dependency Rule strictly**
   - Dependencies must point inward only
   - Inner layers know nothing about outer layers
   - Use interfaces to invert dependencies

2. **DO use CQRS for all operations**
   - Commands for write operations
   - Queries for read operations
   - MediatR for dispatching

3. **DO validate all input at the Application layer**
   - Use FluentValidation
   - Validate before business logic
   - Return meaningful error messages

4. **DO use repositories for data access**
   - Generic repository for CRUD
   - Specific repositories for complex queries
   - Keep EF Core in Persistence layer only

5. **DO use DTOs for API contracts**
   - Never expose domain entities
   - Map entities to DTOs
   - Keep DTOs simple and focused

### Code Quality DO's

1. **DO write clean, readable code**
   ```csharp
   // Good
   var activeProducts = await _repository
       .GetAll()
       .Where(p => p.IsActive && p.StockQuantity > 0)
       .ToListAsync();

   // Avoid
   var x = await _repo.G().Where(y => y.I && y.S > 0).ToListAsync();
   ```

2. **DO handle exceptions properly**
   ```csharp
   try
   {
       await _repository.CreateAsync(entity);
   }
   catch (DbUpdateException ex)
   {
       _logger.LogError(ex, "Database error creating entity");
       throw new BadRequestException("Failed to save entity");
   }
   ```

3. **DO use async/await for async operations**
   ```csharp
   // Correct
   public async Task<Product> GetProductAsync(int id)
   {
       return await _repository.GetByIdAsync(id);
   }

   // Incorrect
   public Product GetProduct(int id)
   {
       return _repository.GetByIdAsync(id).Result;
   }
   ```

4. **DO inject dependencies via constructor**
   ```csharp
   public class ProductService
   {
       private readonly IProductRepository _repository;
       private readonly ILogger<ProductService> _logger;

       public ProductService(IProductRepository repository, ILogger<ProductService> logger)
       {
           _repository = repository ?? throw new ArgumentNullException(nameof(repository));
           _logger = logger ?? throw new ArgumentNullException(nameof(logger));
       }
   }
   ```

5. **DO use meaningful names**
   ```csharp
   // Good
   public async Task<IReadOnlyList<Product>> GetActiveProductsAsync()

   // Avoid
   public async Task<List<obj>> GetStuff()
   ```

### Security DO's

1. **DO use authorization attributes**
   ```csharp
   [Authorize(Roles = "Administrator,Manager")]
   [HttpPost]
   public async Task<IActionResult> CreateProduct(CreateProductCommand command)
   {
       // Implementation
   }
   ```

2. **DO validate user input**
   ```csharp
   public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
   {
       public CreateProductCommandValidator()
       {
           RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
           RuleFor(x => x.Price).GreaterThan(0);
           RuleFor(x => x.Email).EmailAddress();
       }
   }
   ```

3. **DO use parameterized queries**
   ```csharp
   // Good - EF Core handles this automatically
   var product = await _context.Products
       .FirstOrDefaultAsync(p => p.Id == id);

   // Avoid SQL injection
   // Don't use raw SQL with concatenated strings
   ```

4. **DO log security events**
   ```csharp
   _logger.LogWarning("Unauthorized access attempt for user {UserId}", userId);
   ```

### Performance DO's

1. **DO use AsNoTracking for read-only queries**
   ```csharp
   var products = await _context.Products
       .AsNoTracking()
       .Where(p => p.IsActive)
       .ToListAsync();
   ```

2. **DO select only needed columns**
   ```csharp
   var productDtos = await _context.Products
       .Where(p => p.IsActive)
       .Select(p => new ProductDto
       {
           Id = p.Id,
           Name = p.Name,
           Price = p.Price
       })
       .ToListAsync();
   ```

3. **DO use pagination**
   ```csharp
   var products = await _context.Products
       .Skip(pageNumber * pageSize)
       .Take(pageSize)
       .ToListAsync();
   ```

4. **DO use proper indexing**
   ```csharp
   // In entity configuration
   builder.HasIndex(e => e.Name)
       .IsUnique()
       .HasDatabaseName("IX_Products_Name");
   ```

## 🚫 DON'Ts - Avoid These Common Mistakes

### Architecture DON'Ts

1. **DON'T break layer dependencies**
   ```csharp
   // FORBIDDEN - Domain layer with infrastructure dependencies
   public class Product : BaseEntity
   {
       public void Save() // Don't add EF Core methods to entities
       {
           _context.SaveChanges(); // WRONG!
       }
   }
   ```

2. **DON'T skip CQRS for simple operations**
   ```csharp
   // FORBIDDEN - Direct repository access from controller
   [HttpGet]
   public async Task<IActionResult> GetProducts()
   {
       return Ok(await _productRepository.GetAll()); // WRONG!
   }
   ```

3. **DON'T expose domain entities**
   ```csharp
   // FORBIDDEN - Returning domain entity from API
   [HttpGet("{id}")]
   public async Task<Product> GetProduct(int id) // Product is domain entity!
   {
       // WRONG!
   }
   ```

4. **DON'T put business logic in controllers**
   ```csharp
   [HttpPost]
   public async Task<IActionResult> Create(CreateDto dto)
   {
       // FORBIDDEN - Business logic in controller
       if (dto.Price < 0)
       {
           ModelState.AddModelError("Price", "Price must be positive");
           return BadRequest(ModelState);
       }

       // Move to validator/handler
   }
   ```

### Code Quality DON'Ts

1. **DON'T use magic numbers or strings**
   ```csharp
   // Bad
   if (user.Role == 1) // What is 1?

   // Good
   if (user.Role == UserRole.Administrator)

   // Better
   public const int ADMIN_ROLE_ID = 1;
   if (user.Role == ADMIN_ROLE_ID)
   ```

2. **DON'T ignore null checks**
   ```csharp
   // Bad
   public void Process(Product product)
   {
       Console.WriteLine(product.Name); // NullReferenceException!
   }

   // Good
   public void Process(Product? product)
   {
       if (product == null)
           throw new ArgumentNullException(nameof(product));

       Console.WriteLine(product.Name);
   }
   ```

3. **DON'T swallow exceptions**
   ```csharp
   // Bad
   try
   {
       await _repository.SaveAsync();
   }
   catch
   {
       // Do nothing - silent failure
   }

   // Good
   try
   {
       await _repository.SaveAsync();
   }
   catch (DbUpdateException ex)
   {
       _logger.LogError(ex, "Failed to save changes");
       throw; // Re-throw or handle appropriately
   }
   ```

4. **DON'T use async void**
   ```csharp
   // Bad
   public async void Save() // Don't use async void
   {
       await _repository.SaveAsync();
   }

   // Good
   public async Task Save() // Use async Task
   {
       await _repository.SaveAsync();
   }
   ```

### Security DON'Ts

1. **DON'T trust user input**
   ```csharp
   // Bad - Vulnerable to XSS
   var html = $"<div>{userInput}</div>";

   // Good - Encode output
   var html = $"<div>{HtmlEncoder.Encode(userInput)}</div>";
   ```

2. **DON'T hard-code secrets**
   ```csharp
   // Bad
   var apiKey = "secret-key-123";

   // Good
   var apiKey = _configuration["ApiKey"];
   ```

3. **DON'T disable security features**
   ```csharp
   // Bad
   services.Configure<IdentityOptions>(options =>
   {
       options.Password.RequireDigit = false;
       options.Password.RequiredLength = 4; // Too short!
   });
   ```

### Performance DON'Ts

1. **DON'T query unnecessary data**
   ```csharp
   // Bad - Gets all columns
   var products = await _context.Products.ToListAsync();

   // Good - Only needed columns
   var products = await _context.Products
       .Select(p => new { p.Id, p.Name, p.Price })
       .ToListAsync();
   ```

2. **DON'T use N+1 queries**
   ```csharp
   // Bad - N+1 problem
   var orders = await _context.Orders.ToListAsync();
   foreach (var order in orders)
   {
       var customer = await _context.Customers
           .FirstAsync(c => c.Id == order.CustomerId);
   }

   // Good - Eager loading
   var orders = await _context.Orders
       .Include(o => o.Customer)
       .ToListAsync();
   ```

3. **DON'T ignore connection strings**
   ```csharp
   // Bad - Open connection for each operation
   public async Task<Product> GetProduct(int id)
   {
       using var context = new AppDbContext();
       return await context.Products.FindAsync(id);
   }

   // Good - Use context per request
   public class ProductService
   {
       private readonly AppDbContext _context;
       public ProductService(AppDbContext context) => _context = context;
   }
   ```

## 🎯 Golden Rules

1. **Simplicity over complexity**
   - If it's hard to understand, refactor
   - Keep methods under 20 lines
   - One responsibility per class

2. **Consistency is key**
   - Follow established patterns
   - Use consistent naming
   - Keep similar features identical

3. **Testability matters**
   - Write code that's easy to test
   - Use dependency injection
   - Avoid static dependencies

4. **Performance is a feature**
   - Profile before optimizing
   - Use appropriate data structures
   - Consider scalability

5. **Security is everyone's job**
   - Validate all input
   - Use least privilege principle
   - Log security events

## 📊 Decision Matrix

| Situation | Best Practice | Alternative (if needed) |
|-----------|----------------|-------------------------|
| Simple CRUD | CQRS with MediatR | Direct repository (only for very simple cases) |
| Validation | FluentValidation | DataAnnotations (simpler cases) |
| Error Handling | Custom exceptions | Built-in exceptions |
| Logging | Serilog | Microsoft.Extensions.Logging |
| Mapping | AutoMapper | Manual mapping (simple cases) |
| Testing | xUnit + Moq + FluentAssertions | NUnit or MSTest |
| DI | Microsoft.Extensions.DependencyInjection | Autofac (advanced scenarios) |

## 🔍 Code Review Checklist

### Architecture
- [ ] Layer dependencies correct
- [ ] No circular dependencies
- [ ] CQRS pattern followed
- [ ] Repository pattern used
- [ ] DTOs used for API

### Code Quality
- [ ] Naming conventions followed
- [ ] No magic numbers/strings
- [ ] Proper null checking
- [ ] Exception handling implemented
- [ ] Async/await used correctly

### Security
- [ ] Input validation present
- [ ] Authorization applied
- [ ] No hard-coded secrets
- [ ] SQL injection prevented
- [ ] XSS prevented

### Performance
- [ ] Appropriate database queries
- [ ] Pagination implemented
- [ ] Caching considered
- [ ] Indexes defined
- [ ] N+1 queries avoided

## 💡 Quick Reference

### Common Patterns

```csharp
// Controller pattern
[HttpGet("{id}")]
public async Task<ActionResult<EntityDto>> Get(int id)
{
    var result = await _mediator.Send(new GetEntityQuery { Id = id });
    return Ok(result);
}

// Handler pattern
public async Task<EntityDto> Handle(GetEntityQuery request, CancellationToken token)
{
    var entity = await _repository.GetByIdAsync(request.Id);
    if (entity == null)
        throw new NotFoundException(nameof(Entity), request.Id);

    return _mapper.Map<EntityDto>(entity);
}

// Validator pattern
public CreateEntityValidator()
{
    RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(200);

    RuleFor(x => x.Email)
        .EmailAddress();
}
```

Remember: **Good code is written once, read many times**. Write for clarity and maintainability!