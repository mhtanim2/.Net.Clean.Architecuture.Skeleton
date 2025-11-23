using CleanArchitectureApi.Api.Middleware;
using CleanArchitectureApi.Application;
using CleanArchitectureApi.Identity;
using CleanArchitectureApi.Infrastructure;
using CleanArchitectureApi.Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .WriteTo.Console()
    .WriteTo.File("logs/clean-architecture-api-.txt", rollingInterval: RollingInterval.Day)
    .ReadFrom.Configuration(context.Configuration));

// Add services to the container
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());

    // TODO: Configure more restrictive CORS policy for production
    // options.AddPolicy("ProductionPolicy", builder => builder
    //     .WithOrigins("https://yourfrontend.com")
    //     .AllowAnyHeader()
    //     .AllowAnyMethod()
    //     .AllowCredentials());
});

// Add HttpContext accessor
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Clean Architecture API",
        Version = "v1",
        Description = "A clean architecture .NET Web API",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Your Name",
            Email = "your.email@example.com"
        }
    });

    // Include XML Comments if available
    // var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    // var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    // options.IncludeXmlComments(xmlPath);

    // Add JWT Authentication to Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// API Versioning
// builder.Services.AddApiVersioning(options =>
// {
//     options.DefaultApiVersion = new ApiVersion(1, 0);
//     options.AssumeDefaultVersionWhenUnspecified = true;
//     options.ApiVersionReader = ApiVersionReader.Combine(
//         new UrlSegmentApiVersionReader(),
//         new HeaderApiVersionReader("X-Version"),
//         new QueryStringApiVersionReader("version"));
// });

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture API V1");
        options.RoutePrefix = string.Empty; // Set Swagger UI at root
    });
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed data if needed
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var context = services.GetRequiredService<CleanArchitectureDbContext>();
//     var identityContext = services.GetRequiredService<IdentityDbContext>();
//
//     await context.Database.MigrateAsync();
//     await identityContext.Database.MigrateAsync();
//
//     // Seed data
//     // await SeedData.Initialize(services);
// }

try
{
    Log.Information("Starting Clean Architecture API");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}