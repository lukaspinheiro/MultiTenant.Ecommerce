using BROS.Domain.Entities;

namespace BROS.Domain.Interfaces
{
    public interface ITenantRepository
    {
        Task AdicionarAsync(Tenant tenant);
        Task<Tenant?> ObterPorSubdominioAsync(string subdominio);
    }
}
