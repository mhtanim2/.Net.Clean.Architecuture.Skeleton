# Clean Architecture Implementation Guidelines

## 🏛️ Core Principles

### 1. Dependency Rule
**NEVER** allow dependencies to point inward. Always point from outer layers to inner layers:
```
API → Application ← Domain
    ↓           ↓
Infrastructure → Persistence
    ↓
Identity
```

### 2. Layer Responsibilities

#### Domain Layer (Core)
- **Purpose**: Business entities and rules only
- **Dependencies**: NONE (pure .NET)
- **What goes here**:
  - Domain entities inheriting from `BaseEntity`
  - Domain enums
  - Domain-specific exceptions
  - Business logic within entities
- **NEVER add**: EF Core attributes, MediatR, AutoMapper, or any framework dependencies

#### Application Layer
- **Purpose**: Application use cases and orchestration
- **Dependencies**: Domain layer only
- **What goes here**:
  - CQRS Commands and Queries
  - MediatR Handlers
  - DTOs (Data Transfer Objects)
  - AutoMapper profiles
  - FluentValidation validators
  - Service interfaces (contracts)
  - Application exceptions
- **Import pattern**: `using YourProject.Domain;`

#### Persistence Layer
- **Purpose**: Data access implementation
- **Dependencies**: Application layer
- **What goes here**:
  - EF Core DbContext
  - Entity configurations
  - Repository implementations
  - Database migrations
- **Import pattern**: `using YourProject.Application;`

#### Infrastructure Layer
- **Purpose**: External services and cross-cutting concerns
- **Dependencies**: Application layer
- **What goes here**:
  - Email services
  - File storage services
  - Logging implementations
  - Caching services
  - Third-party integrations
- **Import pattern**: `using YourProject.Application;`

#### Identity Layer
- **Purpose**: Authentication and authorization
- **Dependencies**: Application layer
- **What goes here**:
  - ASP.NET Core Identity configuration
  - JWT token services
  - User management services
  - Authentication handlers
- **Import pattern**: `using YourProject.Application;`

#### API Layer
- **Purpose**: HTTP interface and external communication
- **Dependencies**: All other layers
- **What goes here**:
  - Controllers
  - Middleware
  - Filters
  - Swagger configuration
  - Program.cs

## 📋 Mandatory Patterns

### 1. BaseEntity Pattern
ALL domain entities MUST inherit from BaseEntity:
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? DateCreated { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public string? ModifiedBy { get; set; }
}
```

### 2. CQRS with MediatR
ALWAYS use MediatR for operations:

#### Command Pattern:
```csharp
// Command
public class CreateEntityCommand : IRequest<int>
{
    // Properties
}

// Handler
public class CreateEntityCommandHandler : IRequestHandler<CreateEntityCommand, int>
{
    public async Task<int> Handle(CreateEntityCommand request, CancellationToken token)
    {
        // Implementation
    }
}

// Validator
public class CreateEntityCommandValidator : AbstractValidator<CreateEntityCommand>
{
    public CreateEntityCommandValidator()
    {
        // Validation rules
    }
}
```

#### Query Pattern:
```csharp
// Query
public class GetEntityQuery : IRequest<EntityDto>
{
    public int Id { get; set; }
}

// Handler
public class GetEntityQueryHandler : IRequestHandler<GetEntityQuery, EntityDto>
{
    // Implementation
}
```

### 3. Repository Pattern
ALWAYS use repositories for data access:

#### Interface (in Application layer):
```csharp
public interface IEntityRepository : IGenericRepository<Entity>
{
    // Custom methods
}
```

#### Implementation (in Persistence layer):
```csharp
public class EntityRepository : GenericRepository<Entity>, IEntityRepository
{
    // Implementation
}
```

### 4. DTO Pattern
NEVER expose domain entities directly. ALWAYS use DTOs:
```csharp
// Domain Entity (Domain layer)
public class Product : BaseEntity
{
    // Domain properties and logic
}

// DTO (Application layer)
public class ProductDto
{
    // View model properties
}
```

## 🚫 Forbidden Patterns

### NEVER DO:
1. **Domain layer dependencies**:
   ```csharp
   // FORBIDDEN in Domain layer
   using Microsoft.EntityFrameworkCore;
   using MediatR;
   using AutoMapper;
   ```

2. **Circular dependencies**:
   - Domain should NEVER reference any other layer
   - Application should NEVER reference API, Infrastructure, or Persistence

3. **Direct EF Core in Application**:
   ```csharp
   // FORBIDDEN in Application layer
   public class SomeHandler
   {
       private readonly DbContext _context; // WRONG
   }
   ```

4. **Business logic in Controllers**:
   ```csharp
   // FORBIDDEN in API layer
   [HttpPost]
   public async Task<IActionResult> Create(CreateDto dto)
   {
       // Business logic here is WRONG
       if (dto.Price < 0) return BadRequest(); // Move to validator
   }
   ```

5. **Skip validation**:
   ```csharp
   // FORBIDDEN - Always validate
   public async Task<int> Handle(CreateCommand command, CancellationToken token)
   {
       // Missing validation - WRONG
       var entity = _mapper.Map<Entity>(command);
       await _repository.CreateAsync(entity);
   }
   ```

## ✅ Mandatory Checklist for New Features

1. [ ] Domain entity inherits from BaseEntity
2. [ ] Repository interface in Application layer
3. [ ] Repository implementation in Persistence layer
4. [ ] Command/Query classes implement IRequest<T>
5. [ ] All handlers validate input using FluentValidation
6. [ ] DTOs are used for API responses
7. [ ] AutoMapper profile created for entity mappings
8. [ ] Controller methods delegate to MediatR
9. [ ] No business logic in controllers
10. [ ] Proper exception handling implemented

## 🔄 Service Registration Pattern

ALWAYS use extension methods for DI registration:
```csharp
// Application layer
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }
}

// Persistence layer
public static class PersistenceServiceRegistration
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<YourDbContext>(options => {
            // Configuration
        });
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        return services;
    }
}
```

## 🎯 Key Rules to Remember

1. **Layer separation is sacred** - Never break it
2. **Dependencies point inward** - Never the opposite
3. **Domain is pure** - No framework dependencies
4. **CQRS for everything** - No direct repository calls from controllers
5. **Validate first** - Always validate commands before processing
6. **DTOs for API** - Never expose entities
7. **Testability matters** - Code should be easily testable
8. **Consistency over cleverness** - Follow established patterns