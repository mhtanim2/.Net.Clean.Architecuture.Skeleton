# Clean Architecture Web API Skeleton

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/mhtanim2/.Net.Clean.Architecuture.Skeleton/actions)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![Platform](https://img.shields.io/badge/platform-Windows%20%7C%20Linux%20%7C%20macOS-lightgrey.svg)]()

A production-ready Clean Architecture implementation for .NET Web API projects. This skeleton provides a solid foundation for building enterprise-level applications with proper separation of concerns, testability, and maintainability.

**📁 GitHub Repository**: [https://github.com/mhtanim2/.Net.Clean.Architecuture.Skeleton.git](https://github.com/mhtanim2/.Net.Clean.Architecuture.Skeleton.git)

## 🏗️ Architecture Overview

This solution follows Clean Architecture principles with strict layer separation:

```
┌───────────────API Layer─────────────────┐
│                                         │  (Controllers, Middleware)
│    - CleanArchitectureApi.Api           │
|                                         |
├─────────Infrastructure Layer────────────┤
│                                         │  (Data Access, External Services)
│    - CleanArchitectureApi.Persistence   │
│    - CleanArchitectureApi.Infrastructure│
│    - CleanArchitectureApi.Identity      │
|                                         |
├───────────────Core Layer────────────────┤
|                                         |
│               Application               │  (CQRS, DTOs, Validation)
│   - CleanArchitectureApi.Application    │
├  -  -  -  -  -  -  -  -  -  -  -  -  -  ┤
│                Domain                   │  (Entities, Business Logic)
│    - CleanArchitectureApi.Domain        │
└─────────────────────────────────────────┘
```

## ✨ Features

- **Clean Architecture**: Strict separation of concerns with dependency flow
- **CQRS with MediatR**: Command/Query separation for all operations
- **Repository Pattern**: Generic and specific repository implementations
- **Entity Framework Core**: Code-first approach with migrations
- **Authentication & Authorization**: JWT-based auth with ASP.NET Core Identity
- **Validation**: FluentValidation for all commands
- **Exception Handling**: Centralized exception middleware
- **Logging**: Serilog integration with file and console sinks
- **API Documentation**: Swagger/OpenAPI with JWT authentication
- **AutoMapper**: Object-object mapping
- **CORS Configuration**: Cross-origin support
- **Audit Trail**: Automatic Created/Modified tracking
- **Testing Framework**: xUnit with Moq and FluentAssertions

## 🚀 Quick Start

### Prerequisites

- .NET 9.0 SDK or later
- SQL Server (or modify for PostgreSQL/MySQL/SQLite)
- Visual Studio 2022 or VS Code

### 1. Clone and Build

```bash
git clone <repository-url>
cd CleanArchitectureApi
dotnet build
```

### 2. Database Setup

Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=CleanArchitectureApiDb;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

Create and apply migrations:

```bash
dotnet ef migrations add InitialCreate -p CleanArchitectureApi.Persistence -s CleanArchitectureApi.Api
dotnet ef database update -p CleanArchitectureApi.Persistence -s CleanArchitectureApi.Api
```

### 3. Run the Application

```bash
dotnet run --project CleanArchitectureApi.Api
```

The API will be available at `https://localhost:7123` (check console for exact URL).

### 4. Explore API Documentation

Open your browser and navigate to `https://localhost:7123` to view Swagger UI.

## 📸 Screenshots

### Swagger UI with Authentication
![Swagger UI](docs/images/swagger-ui.png)

### Project Structure
![Project Structure](docs/images/project-structure.png)

### Architecture Diagram
![Architecture Diagram](docs/images/architecture-diagram.png)

> **Note**: Screenshots will be added in future updates. You can add your own screenshots in the `docs/images/` folder.

## 📁 Project Structure

```
CleanArchitectureApi/
├── src/
│   ├── Core/
│   │   ├── CleanArchitectureApi.Domain/          # Domain entities and business rules
│   │   │   ├── Common/
│   │   │   │   └── BaseEntity.cs
│   │   │   └── Entities/
│   │   │       └── Product.cs
│   │   └── CleanArchitectureApi.Application/     # CQRS handlers, DTOs, interfaces
│   │       ├── Features/
│   │       │   └── Product/
│   │       ├── Contracts/
│   │       ├── DTOs/
│   │       ├── MappingProfiles/
│   │       └── Exceptions/
│   ├── Infrastructure/
│   │   ├── CleanArchitectureApi.Persistence/     # Data access with EF Core
│   │   ├── CleanArchitectureApi.Infrastructure/ # External services (Email, etc.)
│   │   └── CleanArchitectureApi.Identity/        # Authentication & authorization
│   └── API/
│       └── CleanArchitectureApi.Api/             # Controllers and middleware
└── test/
    ├── CleanArchitectureApi.Application.UnitTests/
    └── CleanArchitectureApi.Persistence.IntegrationTests/
```

## 🔐 Authentication

The application includes a default admin user for testing:

- **Email**: `admin@localhost`
- **Password**: `Admin123!`

### Getting JWT Token

1. Use the `/api/auth/login` endpoint
2. Include the token in subsequent requests:
   ```
   Authorization: Bearer YOUR_JWT_TOKEN
   ```

## 🎯 Sample Implementation: Product Management

This skeleton includes a **complete Product CRUD implementation** demonstrating all Clean Architecture patterns:

### **Included Features:**
- **Domain Entity**: Product with properties (Name, Description, Price, Stock, SKU)
- **CQRS Commands**: Create, Update, Delete with validation
- **CQRS Queries**: GetAll, GetById with pagination support
- **API Endpoints**: Full REST API with role-based authorization
- **Repository Pattern**: Generic repository with specific Product methods
- **Validation**: FluentValidation rules for all operations
- **Mapping**: AutoMapper profiles for DTO transformations

This serves as a perfect reference for implementing your own features following the same patterns.

## 📚 Adding New Features

### 1. Create Domain Entity

```csharp
// Domain/Entities/YourEntity.cs
public class YourEntity : BaseEntity
{
    public string Name { get; set; }
    // Add properties...
}
```

### 2. Create CQRS Commands/Queries

```csharp
// Application/Features/YourEntity/Commands/CreateYourEntity/CreateYourEntityCommand.cs
public class CreateYourEntityCommand : IRequest<int>
{
    public string Name { get; set; }
    // Add properties...
}
```

### 3. Create Handler

```csharp
// Application/Features/YourEntity/Commands/CreateYourEntity/CreateYourEntityCommandHandler.cs
public class CreateYourEntityCommandHandler : IRequestHandler<CreateYourEntityCommand, int>
{
    public async Task<int> Handle(CreateYourEntityCommand request, CancellationToken cancellationToken)
    {
        // Implementation...
    }
}
```

### 4. Create Validator

```csharp
// Application/Features/YourEntity/Commands/CreateYourEntity/CreateYourEntityCommandValidator.cs
public class CreateYourEntityCommandValidator : AbstractValidator<CreateYourEntityCommand>
{
    public CreateYourEntityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        // Add validation rules...
    }
}
```

### 5. Create Controller

```csharp
// API/Controllers/YourEntitiesController.cs
[ApiController]
[Route("api/[controller]")]
public class YourEntitiesController : ControllerBase
{
    private readonly IMediator _mediator;

    public YourEntitiesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateYourEntityCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
    }
}
```

## 🧪 Testing

### Run Unit Tests

```bash
dotnet test CleanArchitectureApi.Application.UnitTests
```

### Run Integration Tests

```bash
dotnet test CleanArchitectureApi.Persistence.IntegrationTests
```

### Run All Tests

```bash
dotnet test
```

## 🔧 Configuration

### Database Providers

The solution supports multiple database providers. Update the configuration in `PersistenceServiceRegistration.cs`:

- **SQL Server** (default)
- **PostgreSQL**
- **MySQL**
- **SQLite**

### JWT Settings

Configure JWT in `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "YOUR_SECRET_KEY_HERE",
    "Issuer": "YourApi",
    "Audience": "YourClients",
    "ExpiresInMinutes": 60
  }
}
```

### Email Settings (Optional)

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-password",
    "FromEmail": "noreply@yourapp.com",
    "FromName": "Your App"
  }
}
```

## 📦 Dependencies

### Core NuGet Packages

- **MediatR**: CQRS implementation
- **AutoMapper**: Object-object mapping
- **FluentValidation**: Validation framework
- **Entity Framework Core**: ORM
- **Serilog**: Structured logging
- **xUnit**: Testing framework
- **Moq**: Mocking framework

## 🌱 Development Workflow

### 1. Create Feature Branch

```bash
git checkout -b feature/your-feature-name
```

### 2. Implement Feature

Follow the CQRS pattern and clean architecture principles.

### 3. Add Tests

Write unit tests for application logic and integration tests for data access.

### 4. Run Tests

```bash
dotnet test
```

### 5. Build and Run

```bash
dotnet build
dotnet run --project CleanArchitectureApi.Api
```

## 🚀 Deployment

### Docker Support

TODO: Add Dockerfile and docker-compose.yml

### Azure DevOps

TODO: Add Azure DevOps pipeline configuration

### GitHub Actions

TODO: Add GitHub Actions workflow

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## 📞 Support

If you have any questions or issues, please create an issue in the repository.

## 🎯 Best Practices Implemented

- ✅ SOLID Principles
- ✅ DRY (Don't Repeat Yourself)
- ✅ Single Responsibility Principle
- ✅ Open/Closed Principle
- ✅ Dependency Inversion Principle
- ✅ Interface Segregation Principle
- ✅ Repository Pattern
- ✅ CQRS Pattern
- ✅ Dependency Injection
- ✅ Unit of Work Pattern
- ✅ DTO Pattern
- ✅ Validation Pattern
- ✅ Exception Handling Pattern
- ✅ Authentication & Authorization
- ✅ Logging Pattern
- ✅ API Versioning Ready
- ✅ Pagination Ready
- ✅ Caching Ready


## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

1. **Fork the repository**
2. **Create a feature branch**: `git checkout -b feature/your-feature-name`
3. **Follow the existing code style and patterns**
4. **Add tests for new features**
5. **Ensure all tests pass**: `dotnet test`
6. **Create a pull request with a clear description**

### Code Style Guidelines

- Use PascalCase for public members
- Use camelCase for private fields
- Add XML comments for public APIs
- Follow Clean Architecture principles
- Keep classes small and focused

## 🔄 Next Steps

- [ ] Add Docker support
- [ ] Implement API versioning
- [ ] Add pagination support
- [ ] Implement caching layer
- [ ] Add file upload support
- [ ] Implement SignalR for real-time features
- [ ] Add rate limiting
- [ ] Implement API key authentication
- [ ] Add health checks
- [ ] Create deployment scripts

---

## 🎉 Enjoy Clean Architecture!

This skeleton provides everything you need to start building enterprise-grade .NET APIs with Clean Architecture. Feel free to customize it according to your project requirements.