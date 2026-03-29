namespace BROS.Domain.Interfaces;

public interface ITenantContext
{
    Guid? TenantId { get; }
    string? Subdominio { get; }
    void SetTenant(Guid id, string subdominio);
}