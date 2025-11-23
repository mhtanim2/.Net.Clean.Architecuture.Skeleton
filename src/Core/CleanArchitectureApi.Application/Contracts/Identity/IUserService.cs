using CleanArchitectureApi.Application.DTOs;

namespace CleanArchitectureApi.Application.Contracts.Identity;

/// <summary>
/// Service interface for user management operations
/// </summary>
public interface IUserService
{
    Task<List<UserDto>> GetUsersAsync();
    Task<UserDto?> GetUserAsync(string userId);
    Task<bool> UpdateUserAsync(string userId, UpdateUserDto updateUserDto);
    Task<bool> DeactivateUserAsync(string userId);
    Task<bool> ActivateUserAsync(string userId);
    Task<bool> AssignRoleAsync(string userId, string roleName);
    Task<bool> RemoveRoleAsync(string userId, string roleName);
    Task<List<string>> GetUserRolesAsync(string userId);
    Task<bool> IsUserInRoleAsync(string userId, string roleName);

    // TODO: Add additional method signatures as needed
    // Task<PaginatedUserDto> GetUsersPaginatedAsync(int pageNumber, int pageSize);
    // Task<List<UserDto>> GetUsersByRoleAsync(string roleName);
    // Task<bool> UpdateUserProfileAsync(string userId, ProfileUpdateDto profileUpdateDto);
    // Task<bool> UpdateUserSecurityInfoAsync(string userId, SecurityUpdateDto securityUpdateDto);
}