using CleanArchitectureApi.Identity.Configurations;
using CleanArchitectureApi.Identity.Models;
using CleanArchitectureApi.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CleanArchitectureApi.Identity;

/// <summary>
/// Service registration for Identity layer
/// </summary>
public static class IdentityServiceRegistration
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure DbContext for Identity
        services.AddDbContext<IdentityDbContext>(options =>
        {
            // TODO: Configure your database provider for Identity
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            // For PostgreSQL:
            // options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

            // For MySQL:
            // options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
            //     ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection")));

            // For SQLite:
            // options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        });

        // Configure Identity
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;

            // Sign-in settings
            options.SignIn.RequireConfirmedEmail = false; // TODO: Set to true in production with email verification
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<IdentityDbContext>()
        .AddDefaultTokenProviders();

        // Configure JWT Authentication
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not configured");
        var key = Encoding.UTF8.GetBytes(secretKey);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; // TODO: Set to true in production
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            };
        });

        // Register identity services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();

        // TODO: Add additional identity configurations
        // services.ConfigureApplicationCookie(options =>
        // {
        //     options.LoginPath = "/api/auth/login";
        //     options.LogoutPath = "/api/auth/logout";
        //     options.AccessDeniedPath = "/api/auth/access-denied";
        // });

        // services.AddAuthorization(options =>
        // {
        //     options.AddPolicy("CanManageProducts", policy =>
        //         policy.RequireRole("Administrator", "ProductManager"));
        //
        //     options.AddPolicy("CanViewOrders", policy =>
        //         policy.RequireRole("Administrator", "Manager", "Sales"));
        // });

        return services;
    }
}