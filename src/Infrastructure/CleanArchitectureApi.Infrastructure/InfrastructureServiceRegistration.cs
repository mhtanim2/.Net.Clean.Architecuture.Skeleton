using CleanArchitectureApi.Application.Contracts.Infrastructure;
using CleanArchitectureApi.Infrastructure.Email;
using CleanArchitectureApi.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureApi.Infrastructure;

/// <summary>
/// Service registration for Infrastructure layer
/// </summary>
public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // TODO: Register email service
        // var emailSettings = configuration.GetSection("EmailSettings");
        // var emailConfig = emailSettings.Get<EmailSettings>();
        // services.AddSingleton(emailConfig!);
        // services.AddScoped<IEmailService, EmailService>();

        // TODO: Register file storage service
        // services.AddScoped<IFileStorageService, LocalFileStorageService>();
        // Or for cloud storage:
        // services.AddScoped<IFileStorageService, AzureBlobStorageService>();
        // services.AddScoped<IFileStorageService, S3StorageService>();

        // TODO: Register caching service
        // services.AddStackExchangeRedisCache(options =>
        // {
        //     options.Configuration = configuration.GetConnectionString("Redis");
        //     options.InstanceName = "CleanArchitecture_";
        // });
        // services.AddScoped<ICacheService, RedisCacheService>();

        // TODO: Register message queue service
        // services.AddScoped<IMessageQueueService, RabbitMQService>();
        // Or for Azure:
        // services.AddScoped<IMessageQueueService, AzureServiceBusService>();

        // Register generic services
        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped<ISystemInfoService, SystemInfoService>();

        return services;
    }
}