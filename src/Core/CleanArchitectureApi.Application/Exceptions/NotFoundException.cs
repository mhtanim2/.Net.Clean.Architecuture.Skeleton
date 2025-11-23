namespace CleanArchitectureApi.Application.Exceptions;

/// <summary>
/// Exception for not found scenarios (404 status code)
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }

    public NotFoundException(string name, object key) : base($"{name} ({key}) was not found")
    {
    }
}