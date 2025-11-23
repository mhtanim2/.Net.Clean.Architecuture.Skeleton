namespace CleanArchitectureApi.Application.DTOs.Auth;

/// <summary>
/// Data Transfer Object for Authentication Response
/// </summary>
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
    public DateTime ExpiresOn { get; set; }

    // TODO: Add additional properties as needed
    // public string RefreshToken { get; set; }
    // public DateTime IssuedOn { get; set; }
    // public string TokenType { get; set; } = "Bearer";
}