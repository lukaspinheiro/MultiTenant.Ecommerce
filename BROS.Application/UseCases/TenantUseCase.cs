using BROS.Application.DTOs;
using BROS.Domain.Entities;
using BROS.Domain.Interfaces;
using System.Text.RegularExpressions;

namespace BROS.Application.UseCases;

public class TenantUseCase : ITenantUseCase
{
    private readonly ITenantRepository _tenantRepository;

    public TenantUseCase(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public async Task ExecuteAsync(CreateTenantRequest request)
    {
        var subdominioLimpo = request.Subdominio.ToLower().Trim();

        if (!Regex.IsMatch(subdominioLimpo, "^[a-z0-9-]+$"))
        {
            throw new ArgumentException("O subdomínio deve conter apenas letras, números e hifens.");
        }

        if (subdominioLimpo.Length < 3)
        {
            throw new ArgumentException("O subdomínio deve ter pelo menos 3 caracteres.");
        }

        var tenantExistente = await _tenantRepository.ObterPorSubdominioAsync(subdominioLimpo);
        if (tenantExistente != null)
        {
            throw new InvalidOperationException($"O subdomínio '{subdominioLimpo}' já está em uso.");
        }

        var novoTenant = new Tenant(request.Nome, subdominioLimpo);

        await _tenantRepository.AdicionarAsync(novoTenant);
    }
}