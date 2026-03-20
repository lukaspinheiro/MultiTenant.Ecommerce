using BROS.Domain.Entities;
using BROS.Domain.Interfaces;
using BROS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BROS.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly ApplicationDbContext _context;

    public TenantRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Tenant tenant)
    {
            await _context.Tenants.AddAsync(tenant);
            await _context.SaveChangesAsync();
    }

    public async Task<Tenant?> ObterPorSubdominioAsync(string subdominio)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Subdominio == subdominio.ToLower());
    }
}
