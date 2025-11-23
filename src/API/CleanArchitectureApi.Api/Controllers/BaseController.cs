using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureApi.Api.Controllers;

/// <summary>
/// Base controller with common functionality
/// </summary>
[ApiController]
[Authorize]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Gets the current user ID from claims
    /// </summary>
    /// <returns>User ID or null if not found</returns>
    protected string? GetCurrentUserId()
    {
        return User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;
    }

    /// <summary>
    /// Gets the current user's email from claims
    /// </summary>
    /// <returns>User email or null if not found</returns>
    protected string? GetCurrentUserEmail()
    {
        return User.FindFirst("email")?.Value;
    }

    /// <summary>
    /// Gets the current user's roles from claims
    /// </summary>
    /// <returns>List of user roles</returns>
    protected List<string> GetCurrentUserRoles()
    {
        return User.FindAll("role").Select(c => c.Value).ToList();
    }

    /// <summary>
    /// Checks if the current user is in the specified role
    /// </summary>
    /// <param name="role">Role name to check</param>
    /// <returns>True if user is in role, false otherwise</returns>
    protected bool IsUserInRole(string role)
    {
        return User.IsInRole(role);
    }

    /// <summary>
    /// Creates a success response with data
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="data">Response data</param>
    /// <param name="message">Optional success message</param>
    /// <returns>Success response</returns>
    protected IActionResult Success<T>(T data, string? message = null)
    {
        var response = new
        {
            success = true,
            message = message ?? "Operation completed successfully",
            data = data,
            timestamp = DateTime.UtcNow
        };

        return Ok(response);
    }

    /// <summary>
    /// Creates an error response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="statusCode">HTTP status code</param>
    /// <returns>Error response</returns>
    protected IActionResult Error(string message, int statusCode = 400)
    {
        var response = new
        {
            success = false,
            message = message,
            timestamp = DateTime.UtcNow
        };

        return StatusCode(statusCode, response);
    }

    /// <summary>
    /// Creates a paginated response
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="data">List of items</param>
    /// <param name="pageNumber">Current page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="totalCount">Total count of items</param>
    /// <returns>Paginated response</returns>
    protected IActionResult PagedResponse<T>(List<T> data, int pageNumber, int pageSize, int totalCount)
    {
        var response = new
        {
            success = true,
            data = data,
            pagination = new
            {
                currentPage = pageNumber,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                totalCount = totalCount,
                hasNextPage = pageNumber * pageSize < totalCount,
                hasPreviousPage = pageNumber > 1
            },
            timestamp = DateTime.UtcNow
        };

        return Ok(response);
    }

    // TODO: Add additional helper methods as needed
    // protected IActionResult CreatedResponse<T>(T data, string location)
    // {
    //     return Created(location, Success(data, "Resource created successfully"));
    // }
    //
    // protected IActionResult NoContentResponse(string message = "Operation completed successfully")
    // {
    //     return Ok(new { success = true, message, timestamp = DateTime.UtcNow });
    // }
}