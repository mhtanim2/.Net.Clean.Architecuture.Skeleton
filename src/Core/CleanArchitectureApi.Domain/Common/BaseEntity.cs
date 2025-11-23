namespace CleanArchitectureApi.Domain.Common;

/// <summary>
/// Base entity for all domain entities with audit fields
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? DateCreated { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    public string? ModifiedBy { get; set; }
}