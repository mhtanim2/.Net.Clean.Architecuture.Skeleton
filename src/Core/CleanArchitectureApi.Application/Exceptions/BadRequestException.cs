using FluentValidation.Results;

namespace CleanArchitectureApi.Application.Exceptions;

/// <summary>
/// Exception for bad request scenarios (400 status code)
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string message, ValidationResult validationResult) : base(message)
    {
        ValidationErrors = validationResult.ToDictionary();
    }

    public IDictionary<string, string[]> ValidationErrors { get; set; }
}