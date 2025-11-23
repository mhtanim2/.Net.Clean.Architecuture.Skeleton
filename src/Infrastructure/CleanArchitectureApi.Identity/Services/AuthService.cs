using CleanArchitectureApi.Application.DTOs;
using CleanArchitectureApi.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CleanArchitectureApi.Identity.Services;

/// <summary>
/// Service for authentication operations
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _roleManager = roleManager;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !user.IsActive)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
            return null;

        // Update last login
        user.UpdateLastLogin();
        await _userManager.UpdateAsync(user);

        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not configured");
        var key = Encoding.UTF8.GetBytes(secretKey);

        var roles = await _userManager.GetRolesAsync(user);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString("R"))
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"] ?? "60")),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var stringToken = tokenHandler.WriteToken(token);

        return new AuthResponseDto
        {
            Token = stringToken,
            UserId = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            FullName = user.FullName,
            Roles = roles.ToList(),
            ExpiresOn = token.ValidTo
        };
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
            return null;

        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
            return null;

        // Assign default role
        await _userManager.AddToRoleAsync(user, "User");

        // Auto-login after registration
        var loginDto = new LoginDto
        {
            Email = registerDto.Email,
            Password = registerDto.Password
        };

        return await LoginAsync(loginDto);
    }

    public async Task<bool> ChangePasswordAsync(string userId, ChangePasswordDto changePasswordDto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return false;

        var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);

        return result.Succeeded;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
        if (user == null)
            return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordDto.NewPassword);

        return result.Succeeded;
    }

    public async Task<bool> LogoutAsync(string userId)
    {
        // TODO: Implement token invalidation or refresh token management
        // In a real implementation, you might:
        // 1. Add the token to a blacklist
        // 2. Invalidate refresh tokens
        // 3. Update user's security stamp

        var user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            return true;
        }

        return false;
    }

    public async Task<UserDto?> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            UserName = user.UserName!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = user.FullName,
            IsActive = user.IsActive,
            Roles = roles.ToList(),
            LastLoginAt = user.LastLoginAt,
            CreatedAt = user.CreatedAt
        };
    }
}