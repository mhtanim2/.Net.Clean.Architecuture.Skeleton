# Feature Development Template and Checklist

## 📝 Template: Adding New Entity Feature

### Step 1: Domain Entity

```csharp
// File: Domain/Entities/[EntityName].cs
using YourProject.Domain.Common;

namespace YourProject.Domain.Entities;

/// <summary>
/// Represents a [EntityName] in the system
/// </summary>
public class [EntityName] : BaseEntity
{
    // Properties
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // TODO: Add specific properties
    // public int CategoryId { get; set; }
    // public decimal Price { get; set; }
    // public bool IsActive { get; set; } = true;

    // Navigation properties
    // public Category Category { get; set; }

    // Domain methods
    // public void Activate()
    // {
    //     IsActive = true;
    //     DateModified = DateTime.UtcNow;
    // }
    //
    // public void Deactivate()
    // {
    //     IsActive = false;
    //     DateModified = DateTime.UtcNow;
    // }
}
```

### Step 2: Application Layer

#### DTOs
```csharp
// File: Application/DTOs/[EntityName]Dto.cs
namespace YourProject.Application.DTOs;

/// <summary>
/// Data Transfer Object for [EntityName]
/// </summary>
public class [EntityName]Dto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime? DateCreated { get; set; }
    public DateTime? DateModified { get; set; }

    // TODO: Add additional properties
    // public int CategoryId { get; set; }
    // public decimal Price { get; set; }
    // public bool IsActive { get; set; }

    // Computed properties
    // public string Status => IsActive ? "Active" : "Inactive";
}
```

#### Repository Interface
```csharp
// File: Application/Contracts/Persistence/I[EntityName]Repository.cs
using YourProject.Domain.Entities;

namespace YourProject.Application.Contracts.Persistence;

public interface I[EntityName]Repository : IGenericRepository<[EntityName]>
{
    // TODO: Add custom repository methods
    // Task<IReadOnlyList<[EntityName]>> GetActive[EntityName]sAsync();
    // Task<[EntityName]?> GetByNameAsync(string name);
    // Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
}
```

#### Create Command
```csharp
// File: Application/Features/[EntityName]/Commands/Create[EntityName]/Create[EntityName]Command.cs
using MediatR;

namespace YourProject.Application.Features.[EntityName].Commands.Create[EntityName];

public class Create[EntityName]Command : IRequest<int>
{
    // TODO: Add command properties
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    // public int CategoryId { get; set; }
    // public decimal Price { get; set; }
    // public bool IsActive { get; set; } = true;
}
```

#### Create Command Validator
```csharp
// File: Application/Features/[EntityName]/Commands/Create[EntityName]/Create[EntityName]CommandValidator.cs
using FluentValidation;

namespace YourProject.Application.Features.[EntityName].Commands.Create[EntityName];

public class Create[EntityName]CommandValidator : AbstractValidator<Create[EntityName]Command>
{
    private readonly I[EntityName]Repository _repository;

    public Create[EntityName]CommandValidator(I[EntityName]Repository repository)
    {
        _repository = repository;

        // TODO: Add validation rules
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters")
            .MustAsync(IsUniqueName).WithMessage("{PropertyName} must be unique");

        RuleFor(p => p.Description)
            .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters");

        // RuleFor(p => p.Price)
        //     .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0");
    }

    private async Task<bool> IsUniqueName(string name, CancellationToken cancellationToken)
    {
        // TODO: Implement uniqueness check
        // return await _repository.IsNameUniqueAsync(name);
        return true;
    }
}
```

#### Create Command Handler
```csharp
// File: Application/Features/[EntityName]/Commands/Create[EntityName]/Create[EntityName]CommandHandler.cs
using AutoMapper;
using YourProject.Application.Contracts.Persistence;
using YourProject.Application.Exceptions;
using YourProject.Domain.Entities;
using MediatR;

namespace YourProject.Application.Features.[EntityName].Commands.Create[EntityName];

public class Create[EntityName]CommandHandler : IRequestHandler<Create[EntityName]Command, int>
{
    private readonly IMapper _mapper;
    private readonly I[EntityName]Repository _repository;

    public Create[EntityName]CommandHandler(IMapper mapper, I[EntityName]Repository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<int> Handle(Create[EntityName]Command request, CancellationToken cancellationToken)
    {
        // Validate input
        var validator = new Create[EntityName]CommandValidator(_repository);
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new BadRequestException("Invalid [EntityName] data", validationResult);

        // Map to domain entity
        var entity = _mapper.Map<[EntityName]>(request);

        // TODO: Set additional properties
        // entity.CreatedAt = DateTime.UtcNow;

        // Create entity
        await _repository.CreateAsync(entity);

        // Return ID
        return entity.Id;
    }
}
```

#### Get By ID Query
```csharp
// File: Application/Features/[EntityName]/Queries/Get[EntityName]ById/Get[EntityName]ByIdQuery.cs
using MediatR;

namespace YourProject.Application.Features.[EntityName].Queries.Get[EntityName]ById;

public class Get[EntityName]ByIdQuery : IRequest<[EntityName]Dto>
{
    public int Id { get; set; }
}

// File: Application/Features/[EntityName]/Queries/Get[EntityName]ById/Get[EntityName]ByIdQueryHandler.cs
using AutoMapper;
using YourProject.Application.Contracts.Persistence;
using YourProject.Application.Exceptions;
using MediatR;

namespace YourProject.Application.Features.[EntityName].Queries.Get[EntityName]ById;

public class Get[EntityName]ByIdQueryHandler : IRequestHandler<Get[EntityName]ByIdQuery, [EntityName]Dto>
{
    private readonly IMapper _mapper;
    private readonly I[EntityName]Repository _repository;

    public Get[EntityName]ByIdQueryHandler(IMapper mapper, I[EntityName]Repository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<[EntityName]Dto> Handle(Get[EntityName]ByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id);

        if (entity == null)
            throw new NotFoundException(nameof([EntityName]), request.Id);

        return _mapper.Map<[EntityName]Dto>(entity);
    }
}
```

#### AutoMapper Profile
```csharp
// File: Application/MappingProfiles/[EntityName]Profile.cs
using AutoMapper;
using YourProject.Application.DTOs;
using YourProject.Domain.Entities;

namespace YourProject.Application.MappingProfiles;

public class [EntityName]Profile : Profile
{
    public [EntityName]Profile()
    {
        CreateMap<[EntityName], [EntityName]Dto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<Create[EntityName]Command, [EntityName]>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.DateModified, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedBy, opt => opt.Ignore());

        // TODO: Add additional mappings
        // CreateMap<Update[EntityName]Command, [EntityName]>()
        //     .ForMember(dest => dest.Id, opt => opt.Ignore())
        //     .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
        //     .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
    }
}
```

### Step 3: Persistence Layer

#### Repository Implementation
```csharp
// File: Persistence/Repositories/[EntityName]Repository.cs
using YourProject.Application.Contracts.Persistence;
using YourProject.Domain.Entities;
using YourProject.Persistence.DatabaseContext;

namespace YourProject.Persistence.Repositories;

public class [EntityName]Repository : GenericRepository<[EntityName]>, I[EntityName]Repository
{
    public [EntityName]Repository([YourProject]DbContext context) : base(context)
    {
    }

    // TODO: Implement custom repository methods
    // public async Task<IReadOnlyList<[EntityName]>> GetActive[EntityName]sAsync()
    // {
    //     return await _context.[EntityName]s
    //         .Where(e => e.IsActive)
    //         .AsNoTracking()
    //         .ToListAsync();
    // }
}
```

#### Entity Configuration
```csharp
// File: Persistence/Configurations/[EntityName]Configuration.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Domain.Entities;

namespace YourProject.Persistence.Configurations;

public class [EntityName]Configuration : IEntityTypeConfiguration<[EntityName]>
{
    public void Configure(EntityTypeBuilder<[EntityName]> builder)
    {
        builder.ToTable("[EntityName]s");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        // TODO: Add configurations
        // builder.Property(e => e.Price)
        //     .HasColumnType("decimal(18,2)");

        // Indexes
        builder.HasIndex(e => e.Name)
            .IsUnique()
            .HasDatabaseName("IX_[EntityName]s_Name");

        // Relationships
        // builder.HasOne(e => e.Category)
        //     .WithMany()
        //     .HasForeignKey(e => e.CategoryId)
        //     .OnDelete(DeleteBehavior.Restrict);
    }
}
```

### Step 4: API Layer

#### Controller
```csharp
// File: API/Controllers/[EntityName]sController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourProject.Application.DTOs;
using YourProject.Application.Features.[EntityName].Commands.Create[EntityName];
using YourProject.Application.Features.[EntityName].Commands.Update[EntityName];
using YourProject.Application.Features.[EntityName].Commands.Delete[EntityName];
using YourProject.Application.Features.[EntityName].Queries.Get[EntityName]ById;
using YourProject.Application.Features.[EntityName].Queries.GetAll[EntityName]s;
using MediatR;

namespace YourProject.Api.Controllers;

/// <summary>
/// Controller for managing [EntityName]s
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class [EntityName]sController : ControllerBase
{
    private readonly IMediator _mediator;

    public [EntityName]sController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all [EntityName]s
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<[EntityName]Dto>>> Get[EntityName]s()
    {
        var result = await _mediator.Send(new GetAll[EntityName]sQuery());
        return Ok(result);
    }

    /// <summary>
    /// Get [EntityName] by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<[EntityName]Dto>> Get[EntityName](int id)
    {
        var result = await _mediator.Send(new Get[EntityName]ByIdQuery { Id = id });
        return Ok(result);
    }

    /// <summary>
    /// Create new [EntityName]
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<ActionResult<int>> Create[EntityName](Create[EntityName]Command command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get[EntityName]), new { id = result }, result);
    }

    /// <summary>
    /// Update [EntityName]
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Administrator,Manager")]
    public async Task<ActionResult> Update[EntityName](int id, Update[EntityName]Command command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Delete [EntityName]
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrator")]
    public async Task<ActionResult> Delete[EntityName](int id)
    {
        await _mediator.Send(new Delete[EntityName]Command { Id = id });
        return NoContent();
    }
}
```

### Step 5: Service Registration

Update `PersistenceServiceRegistration.cs`:
```csharp
// Add to services
services.AddScoped<I[EntityName]Repository, [EntityName]Repository>();
```

## 📋 Feature Development Checklist

### Phase 1: Planning
- [ ] Understand the feature requirements completely
- [ ] Identify all entities involved
- [ ] Plan the CRUD operations needed
- [ ] Check for similar existing features
- [ ] Create TodoWrite list for tracking

### Phase 2: Domain Layer
- [ ] Create/Update domain entity in `Domain/Entities/`
- [ ] Ensure entity inherits from `BaseEntity`
- [ ] Add domain properties and navigation properties
- [ ] Add domain methods for business logic
- [ ] Add domain-specific validation if needed

### Phase 3: Application Layer
- [ ] Create DTOs in `Application/DTOs/`
- [ ] Create repository interface in `Application/Contracts/Persistence/`
- [ ] Create CQRS commands and queries in `Application/Features/`
  - [ ] Create folder structure: `[EntityName]/Commands/[Action]/`
  - [ ] Create folder structure: `[EntityName]/Queries/[Action]/`
- [ ] Create FluentValidation validators
- [ ] Create MediatR handlers
- [ ] Update/create AutoMapper profiles
- [ ] Handle all exceptions appropriately

### Phase 4: Persistence Layer
- [ ] Create repository implementation in `Persistence/Repositories/`
- [ ] Create entity configuration in `Persistence/Configurations/`
- [ ] Add DbSet to DbContext if new entity
- [ ] Register repository in DI container

### Phase 5: API Layer
- [ ] Create/update controller in `API/Controllers/`
- [ ] Add HTTP verb attributes
- [ ] Add route attributes
- [ ] Add authorization attributes
- [ ] Add XML documentation comments
- [ ] Return appropriate HTTP status codes
- [ ] Use CreatedAtAction for POST operations

### Phase 6: Testing
- [ ] Create unit tests for handlers
- [ ] Create integration tests for repositories
- [ ] Test all endpoints with Swagger
- [ ] Test validation rules
- [ ] Test error scenarios

### Phase 7: Final Review
- [ ] Check code follows all standards
- [ ] Verify Clean Architecture rules
- [ ] Check naming conventions
- [ ] Verify error handling
- [ ] Check for TODO comments
- [ ] Update documentation if needed

## 🚫 Common Mistakes to Avoid

1. **Skipping validation** - Always validate input
2. **Business logic in controllers** - Keep logic in handlers
3. **Direct EF Core in Application layer** - Use repositories
4. **Missing error handling** - Handle all exceptions
5. **Inconsistent naming** - Follow conventions
6. **Not checking nulls** - Validate inputs
7. **Forgetting authorization** - Secure endpoints
8. **Missing XML docs** - Document public APIs

## 💡 Tips and Tricks

1. **Use snippets** for repetitive code
2. **Copy existing features** as templates
3. **Test as you go**, don't wait until the end
4. **Keep methods small** and focused
5. **Use meaningful names** for everything
6. **Write self-documenting code**
7. **Consider edge cases** in validation
8. **Plan for future extensibility**