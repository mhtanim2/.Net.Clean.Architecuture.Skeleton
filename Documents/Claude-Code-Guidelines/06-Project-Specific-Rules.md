# Project-Specific Rules and Patterns

## 🏗️ HR.LeaveManagement.Clean Project Patterns

This document captures the specific patterns and rules extracted from the HR.LeaveManagement.Clean project that should be followed consistently across all Clean Architecture API projects.

## 📋 Mandatory Project Structure

### Solution Folder Organization
```
SolutionName.sln
├── src/
│   ├── Core/
│   │   ├── YourProject.Domain/          # Pure domain entities
│   │   └── YourProject.Application/     # CQRS, DTOs, validation
│   ├── Infrastructure/
│   │   ├── YourProject.Persistence/     # EF Core implementation
│   │   ├── YourProject.Infrastructure/ # External services
│   │   └── YourProject.Identity/        # Auth & authorization
│   └── API/
│       └── YourProject.Api/             # Controllers, middleware
└── test/
    ├── YourProject.Application.UnitTests/
    └── YourProject.Persistence.IntegrationTests/
```

### Naming Conventions
- **Project names**: `YourProject.LayerName` (e.g., `CleanArchitectureApi.Domain`)
- **Solution groups**: Organize by layer (Core, Infrastructure, API)
- **Folder structure**: Follow feature-based organization in Application layer

## 🔧 Technology Stack Rules

### Must-Use Libraries
1. **MediatR** - For all CQRS operations
2. **AutoMapper** - For object mapping
3. **FluentValidation** - For validation
4. **Entity Framework Core** - For data persistence
5. **ASP.NET Core Identity** - For authentication
6. **Serilog** - For logging
7. **xUnit** - For testing
8. **Moq** - For mocking in tests

### Package Versions (As of .NET 9.0)
```xml
<PackageReference Include="AutoMapper" Version="13.0.1" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.11.0" />
<PackageReference Include="FluentValidation.DependencyInjectionExtensions" Version="11.11.0" />
<PackageReference Include="MediatR" Version="12.8.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
<PackageReference Include="Serilog" Version="4.1.0" />
```

## 📝 Code Patterns from HR.LeaveManagement

### 1. BaseEntity Pattern
```csharp
// In Domain/Common/BaseEntity.cs
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? DateCreated { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public string? ModifiedBy { get; set; }
}
```

### 2. Service Registration Extension Pattern
```csharp
// Each layer has its own extension method
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
```

### 3. Exception Handling Pattern
```csharp
// Custom exceptions in Application layer
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }

    public BadRequestException(string message, ValidationResult validationResult)
        : base(message)
    {
        ValidationErrors = validationResult.ToDictionary();
    }

    public IDictionary<string, string[]>? ValidationErrors { get; set; }
}

// Global middleware in API layer
public class ExceptionMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
}
```

### 4. Repository Pattern Implementation
```csharp
// Generic repository interface
public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> GetAsync();
    Task<T> GetByIdAsync(int id);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}

// Specific repository interface
public interface ILeaveTypeRepository : IGenericRepository<LeaveType>
{
    // Custom methods
}

// Repository implementation
public class LeaveTypeRepository : GenericRepository<LeaveType>, ILeaveTypeRepository
{
    // Implementation
}
```

### 5. CQRS Handler Pattern
```csharp
// Command example
public class CreateLeaveTypeCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public int DefaultDays { get; set; }
}

// Handler example
public class CreateLeaveTypeCommandHandler : IRequestHandler<CreateLeaveTypeCommand, int>
{
    public async Task<int> Handle(CreateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateLeaveTypeCommandValidator(_leaveTypeRepository);
        var validationResult = await validator.ValidateAsync(request);

        if (validationResult.Errors.Any())
            throw new BadRequestException("Invalid Leave type", validationResult);

        var leaveTypeToCreate = _mapper.Map<Domain.LeaveType>(request);
        await _leaveTypeRepository.CreateAsync(leaveTypeToCreate);

        return leaveTypeToCreate.Id;
    }
}
```

### 6. Validation Pattern
```csharp
public class CreateLeaveTypeCommandValidator : AbstractValidator<CreateLeaveTypeCommand>
{
    private readonly ILeaveTypeRepository _leaveTypeRepository;

    public CreateLeaveTypeCommandValidator(ILeaveTypeRepository leaveTypeRepository)
    {
        _leaveTypeRepository = leaveTypeRepository;

        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("{PropertyName} is required")
            .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters");
    }
}
```

## 🗄️ Database Patterns

### EF Core DbContext Pattern
```csharp
public class HrDatabaseContext : DbContext
{
    // Automatic audit fields in SaveChangesAsync
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.DateCreated = DateTime.UtcNow;
                    entry.Entity.CreatedBy = GetCurrentUserId();
                    break;
                case EntityState.Modified:
                    entry.Property(e => e.DateCreated).IsModified = false;
                    entry.Property(e => e.CreatedBy).IsModified = false;
                    entry.Entity.DateModified = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = GetCurrentUserId();
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
```

### Entity Configuration Pattern
```csharp
public class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
{
    public void Configure(EntityTypeBuilder<LeaveType> builder)
    {
        builder.ToTable("LeaveTypes");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.DefaultDays)
            .IsRequired();
    }
}
```

## 🔐 Authentication & Authorization Pattern

### JWT Configuration
```csharp
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
```

### Identity User Pattern
```csharp
public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
}

public class ApplicationRole : IdentityRole
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
```

## 🌐 API Patterns

### Controller Pattern
```csharp
[ApiController]
[Route("api/[controller]")]
public class LeaveTypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaveTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<LeaveTypeDto>>> Get()
    {
        var leaveTypes = await _mediator.Send(new GetLeaveTypesQuery());
        return Ok(leaveTypes);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateLeaveTypeCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id }, new { Id = id });
    }
}
```

### CORS Configuration
```csharp
services.AddCors(options =>
{
    options.AddPolicy("all", builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});
```

## 📊 Configuration Patterns

### appsettings.json Structure
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=YourDb;Trusted_Connection=true"
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKey",
    "Issuer": "YourApi",
    "Audience": "YourClients",
    "ExpiresInMinutes": 60
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    }
  }
}
```

### Program.cs Pattern
```csharp
var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .WriteTo.Console()
    .ReadFrom.Configuration(context.Configuration));

// Services
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

// Middleware
app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseAuthentication();
app.UseAuthorization();
```

## 🧪 Testing Patterns

### Unit Test Pattern
```csharp
public class CreateLeaveTypeCommandTests
{
    [Fact]
    public async Task Handle_ValidRequest_ReturnsLeaveTypeId()
    {
        // Arrange
        var mapperMock = new Mock<IMapper>();
        var repositoryMock = new Mock<ILeaveTypeRepository>();
        var handler = new CreateLeaveTypeCommandHandler(mapperMock.Object, repositoryMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeGreaterThan(0);
    }
}
```

## 🎯 Project-Specific Rules Summary

1. **Always follow HR.LeaveManagement patterns** - They are the reference implementation
2. **Use the exact same folder structure** - Consistency across projects
3. **Implement all mandatory patterns** - BaseEntity, ServiceRegistration, etc.
4. **Use the specified library versions** - Keep dependencies aligned
5. **Follow the naming conventions exactly** - No deviations
6. **Implement all cross-cutting concerns** - Logging, exceptions, auth
7. **Use the same validation approach** - FluentValidation for everything
8. **Follow the database patterns** - Audit fields, configurations, migrations

## 🔍 Migration Checklist

When starting a new project based on these patterns:

1. [ ] Create the exact folder structure
2. [ ] Add all NuGet packages with specified versions
3. [ ] Copy BaseService class patterns
4. [ ] Set up ServiceRegistration extensions
5. [ ] Configure Program.cs with all middleware
6. [ ] Set up exception handling middleware
7. [ ] Configure JWT and Identity
8. [ ] Set up Serilog logging
9. [ ] Create BaseEntity
10. [ ] Set up repository pattern
11. [ ] Configure Swagger with JWT
12. [ ] Create initial DTO and Entity templates

Remember: **Consistency over creativity**. These patterns have been proven to work well - stick to them!