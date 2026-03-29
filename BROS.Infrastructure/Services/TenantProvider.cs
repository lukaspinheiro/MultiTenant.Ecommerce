using BROS.Domain.Interfaces;

namespace BROS.Infrastructure.Services;

public class TenantProvider : ITenantProvider
{
    private readonly ITenantContext _tenantContext;

    public TenantProvider(ITenantContext tenantContext)
    {
        _tenantContext = tenantContext;
    }

    public Guid? ObterTenantId()
    {
        return _tenantContext.TenantId;
    }

}