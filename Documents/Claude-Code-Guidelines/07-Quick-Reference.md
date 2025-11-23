# Quick Reference for Claude Code

## 🚀 Claude Code Instructions

### When Starting Any Task

1. **FIRST: Read these guidelines**
   - Open and read `./Documents/Claude-Code-Guidelines/README.md`
   - Understand the project structure and rules

2. **THEN: Analyze the codebase**
   ```bash
   # Find existing patterns
   find . -name "*.cs" -type f | head -20

   # Check project structure
   ls -la src/

   # Look for similar features
   grep -r "class.*Controller" --include="*.cs" .
   ```

3. **ALWAYS: Create TodoWrite list** for complex tasks

## 📋 Quick Start Templates

### Adding New Entity
```csharp
// 1. Domain/Entities/YourEntity.cs
public class YourEntity : BaseEntity
{
    public string Name { get; set; }
}

// 2. Application/DTOs/YourEntityDto.cs
public class YourEntityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// 3. Application/Contracts/Persistence/IYourEntityRepository.cs
public interface IYourEntityRepository : IGenericRepository<YourEntity>
{
}

// 4. Application/Features/YourEntity/Commands/CreateYourEntity/CreateYourEntityCommand.cs
public class CreateYourEntityCommand : IRequest<int>
{
    public string Name { get; set; }
}

// 5. Application/Features/YourEntity/Commands/CreateYourEntity/CreateYourEntityCommandHandler.cs
public class CreateYourEntityCommandHandler : IRequestHandler<CreateYourEntityCommand, int>
{
    // Validate → Map → Create → Return ID
}

// 6. Application/Features/YourEntity/Commands/CreateYourEntity/CreateYourEntityCommandValidator.cs
public class CreateYourEntityCommandValidator : AbstractValidator<CreateYourEntityCommand>
{
    public CreateYourEntityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

// 7. Persistence/Repositories/YourEntityRepository.cs
public class YourEntityRepository : GenericRepository<YourEntity>, IYourEntityRepository
{
}

// 8. Persistence/Configurations/YourEntityConfiguration.cs
public class YourEntityConfiguration : IEntityTypeConfiguration<YourEntity>
{
    public void Configure(EntityTypeBuilder<YourEntity> builder)
    {
        builder.ToTable("YourEntities");
    }
}

// 9. API/Controllers/YourEntitiesController.cs
[ApiController]
[Route("api/[controller]")]
public class YourEntitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateYourEntityCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id = result }, result);
    }
}
```

## ✅ Mandatory Checks

### Before Coding
- [ ] Read the request completely
- [ ] Analyze existing codebase
- [ ] Create TodoWrite list
- [ ] Plan the solution

### While Coding
- [ ] Follow Clean Architecture rules
- [ ] Use CQRS with MediatR
- [ ] Add FluentValidation
- [ ] Handle exceptions properly
- [ ] Use AutoMapper for mapping

### After Coding
- [ ] Check layer dependencies
- [ ] Verify naming conventions
- [ ] Test with Swagger
- [ ] Check for TODOs
- [ ] Update documentation

## 🎯 Common Commands

### Project Structure
```bash
# View solution structure
tree -L 3

# Find all controllers
find . -name "*Controller.cs" -type f

# Find all entities
find . -path "*/Domain/Entities/*.cs" -type f

# Search for specific patterns
grep -r "IRequestHandler" --include="*.cs" .
```

### Build and Test
```bash
# Build solution
dotnet build

# Run tests
dotnet test

# Run specific project
dotnet run --project src/API/YourProject.Api

# Create migration
dotnet ef migrations add MigrationName -p Persistence -s API
```

### Code Quality
```bash
# Check for TODO comments
grep -r "TODO" --include="*.cs" .

# Find large files
find . -name "*.cs" -size +10k

# Count lines of code
find . -name "*.cs" -type f | xargs wc -l
```

## 🔧 Essential Patterns

### CQRS Handler Pattern
```csharp
public class YourCommandHandler : IRequestHandler<YourCommand, ReturnType>
{
    public async Task<ReturnType> Handle(YourCommand request, CancellationToken token)
    {
        // 1. Validate
        var validator = new YourCommandValidator();
        var result = await validator.ValidateAsync(request);

        if (!result.IsValid)
            throw new BadRequestException("Invalid data", result);

        // 2. Map/Process
        var entity = _mapper.Map<YourEntity>(request);

        // 3. Persist
        await _repository.CreateAsync(entity);

        // 4. Return
        return entity.Id;
    }
}
```

### Validator Pattern
```csharp
public class YourValidator : AbstractValidator<YourCommand>
{
    public YourValidator()
    {
        RuleFor(x => x.Property)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUnique).WithMessage("Must be unique");
    }
}
```

### Repository Pattern
```csharp
// Interface (Application layer)
public interface IYourRepository : IGenericRepository<YourEntity>
{
    Task<IReadOnlyList<YourEntity>> GetActiveAsync();
}

// Implementation (Persistence layer)
public class YourRepository : GenericRepository<YourEntity>, IYourRepository
{
    public async Task<IReadOnlyList<YourEntity>> GetActiveAsync()
    {
        return await _context.YourEntities
            .Where(x => x.IsActive)
            .AsNoTracking()
            .ToListAsync();
    }
}
```

## 🚫 Things to Never Do

```csharp
// NEVER add framework dependencies to Domain
using Microsoft.EntityFrameworkCore; // FORBIDDEN in Domain layer
using MediatR; // FORBIDDEN in Domain layer

// NEVER expose entities directly
[HttpGet]
public async Task<Product> Get(int id) // WRONG - Use DTO
{
    return await _repository.GetByIdAsync(id);
}

// NEVER skip validation
public async Task Create(CreateCommand command) // WRONG - Validate first
{
    // Direct processing without validation
}

// NEVER add business logic to controllers
[HttpPost]
public async Task Create(CreateDto dto) // WRONG
{
    if (dto.Price < 0) // Move to validator
        return BadRequest();
}
```

## 📝 Quick Validator Rules

```csharp
// Common validation rules
RuleFor(x => x.Name)
    .NotEmpty()
    .MaximumLength(200);

RuleFor(x => x.Email)
    .NotEmpty()
    .EmailAddress();

RuleFor(x => x.Price)
    .GreaterThan(0)
    .LessThan(10000);

RuleFor(x => x.Date)
    .GreaterThanOrEqualTo(DateTime.Today);

// Custom validation
RuleFor(x => x.Username)
    .MustAsync(BeUniqueUsername)
    .WithMessage("Username already exists");
```

## 🏗️ Service Registration Pattern

```csharp
// In each layer
public static class LayerServiceRegistration
{
    public static IServiceCollection AddLayerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register services
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}

// In Program.cs
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
```

## 🔍 Debugging Checklist

1. **Check if building**
   ```bash
   dotnet build
   ```

2. **Check dependencies**
   - Are all references correct?
   - Any circular dependencies?

3. **Check layer rules**
   - Domain has no external deps?
   - Application only references Domain?
   - Persistence references Application?

4. **Check CQRS**
   - Command/Query implements IRequest?
   - Handler implements IRequestHandler?
   - Validator exists?

5. **Check API**
   - Controller delegates to MediatR?
   - Proper HTTP verbs used?
   - Authorization attributes?

## 💡 Pro Tips

1. **Always use async/await** for async operations
2. **Use nameof()** instead of magic strings
3. **Prefer expression-bodied members** for simple methods
4. **Use null propagation** (?.) and null coalescing (??)
5. **String interpolation** over string concatenation
6. **ConfigureAwait(false)** in library code
7. **Use IReadOnlyList** for read-only returns
8. **CancellationToken** for async methods

## 📞 Get Help

When stuck:
1. Re-read the guidelines
2. Check HR.LeaveManagement.Clean for reference
3. Look for similar implemented features
4. Ask for clarification if needed

Remember: **Consistency over cleverness!**