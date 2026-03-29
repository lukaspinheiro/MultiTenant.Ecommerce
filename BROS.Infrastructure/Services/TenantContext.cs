using BROS.Domain.Interfaces;

namespace BROS.Infrastructure.Services;

public class TenantContext : ITenantContext
{
    public Guid? TenantId { get; private set; }
    public string? Subdominio { get; private set; }

    public void SetTenant(Guid id, string subdominio)
    {
        TenantId = id;
        Subdominio = subdominio;
    }
}