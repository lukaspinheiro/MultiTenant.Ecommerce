using BROS.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BROS.Infrastructure.Services;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _tenantId;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetTenantId()
    {
        // 1. Se está em cache, retorna o valor
        if (_tenantId.HasValue) return _tenantId;

        // 2. Se não tem contexto HTTP, tá em design time ou contexto não web
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return null;

        // 3. Extrai o subdomínio da requisição (ex: "loja1.seuecommerce.com")
        var host = context.Request.Host.Host;
        var subdominio = host.Split('.')[0];

        // 4. Aqui você deve buscar no banco ou cache o ID do tenant pelo subdomínio.
        // Como o DbContext depende do TenantProvider, não podemos injetar o DbContext aqui (risco de dependência circular).
        // Solução Profissional: Usar um IMemoryCache ou uma busca rápida por um cache local.

        // Por hora, simularemos a resolução:
        _tenantId = ResolverTenantPorSubdominio(subdominio);

        return _tenantId;
    }

    private Guid? ResolverTenantPorSubdominio(string subdominio)
    {
        // Em produção, aqui você consultaria um cache (Redis ou MemoryCache)
        // Para simplificar agora, faremos a lógica de busca quando avançarmos
        return Guid.Empty; 
    }
}