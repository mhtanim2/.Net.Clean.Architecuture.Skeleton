using CleanArchitectureApi.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace CleanArchitectureApi.Api.Middleware;

/// <summary>
/// Middleware for handling exceptions globally
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.Clear();
        context.Response.ContentType = "application/json";

        var response = new Dictionary<string, object>
        {
            ["timestamp"] = DateTime.UtcNow,
            ["path"] = context.Request.Path
        };

        switch (exception)
        {
            case BadRequestException badRequestEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response["status"] = (int)HttpStatusCode.BadRequest;
                response["error"] = "Bad Request";
                response["message"] = badRequestEx.Message;

                if (badRequestEx.ValidationErrors != null)
                {
                    response["validationErrors"] = badRequestEx.ValidationErrors;
                }
                break;

            case NotFoundException notFoundEx:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response["status"] = (int)HttpStatusCode.NotFound;
                response["error"] = "Not Found";
                response["message"] = notFoundEx.Message;
                break;

            case UnauthorizedAccessException unauthorizedEx:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response["status"] = (int)HttpStatusCode.Unauthorized;
                response["error"] = "Unauthorized";
                response["message"] = unauthorizedEx.Message;
                break;

            case KeyNotFoundException keyNotFoundEx:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response["status"] = (int)HttpStatusCode.NotFound;
                response["error"] = "Not Found";
                response["message"] = keyNotFoundEx.Message;
                break;

            case ArgumentException argEx:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response["status"] = (int)HttpStatusCode.BadRequest;
                response["error"] = "Bad Request";
                response["message"] = argEx.Message;
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response["status"] = (int)HttpStatusCode.InternalServerError;
                response["error"] = "Internal Server Error";
                response["message"] = "An unexpected error occurred. Please try again later.";

                // Include stack trace only in development
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    response["exception"] = exception.Message;
                    response["stackTrace"] = exception.StackTrace;
                }
                break;
        }

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var jsonResponse = JsonSerializer.Serialize(response, jsonOptions);
        await context.Response.WriteAsync(jsonResponse);
    }
}