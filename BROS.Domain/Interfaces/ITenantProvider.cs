namespace BROS.Domain.Interfaces;

public interface ITenantProvider
{
    Guid? GetTenantId();
}