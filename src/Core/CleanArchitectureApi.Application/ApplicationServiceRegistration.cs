using AutoMapper;
using CleanArchitectureApi.Application.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanArchitectureApi.Application;

/// <summary>
/// Service registration for Application layer
/// </summary>
public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Register MediatR
        services.AddMediatR(Assembly.GetExecutingAssembly());

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // TODO: Add other application services here
        // services.AddScoped<IExampleService, ExampleService>();

        return services;
    }
}