using BROS.Domain.Interfaces;

namespace BROS.Api.Middlewares;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITenantRepository repository, ITenantContext tenantContext)
    {
        if (context.Request.Headers.TryGetValue("X-Tenant", out var tenantHeader))
        {
            var subdominio = tenantHeader.ToString();

            var tenant = await repository.ObterPorSubdominioAsync(subdominio);

            if (tenant != null)
            {
                tenantContext.SetTenant(tenant.Id, tenant.Subdominio);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { erro = "Lojista não encontrado ou inválido." });
                return;
            }
        }

        await _next(context);
    }
}