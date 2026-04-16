using BROS.Domain.Entities;
using BROS.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BROS.Application.UseCases.Lojistas.RegistrarLojista;

public class RegistrarLojistaUseCase : IRegistrarLojistaUseCase
{
    private readonly ITenantRepository _tenantRepository;
    private readonly UserManager<Usuario> _userManager;
    private readonly ITenantContext _tenantContext;
    private readonly IUnitOfWork _unitOfWork;

    public RegistrarLojistaUseCase(
        ITenantRepository tenantRepository,
        UserManager<Usuario> userManager,
        ITenantContext tenantContext,
        IUnitOfWork unitOfWork)
    {
        _tenantRepository = tenantRepository;
        _userManager = userManager;
        _tenantContext = tenantContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<RegistrarLojistaResponse> ExecutarAsync(RegistrarLojistaRequest request)
    {
        var existente = await _tenantRepository.ObterPorSubdominioAsync(request.Subdominio);
        if (existente != null)
        {
            return new RegistrarLojistaResponse(false, null, "Este subdomínio já está em uso.");
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            // Cria Entidade Tenant usando construtor
            var tenant = new Tenant(request.NomeLoja, request.Subdominio.ToLower());

            // Após criar Tenant, injeta o TenantId no contexto.
            _tenantContext.SetTenant(tenant.Id, tenant.Subdominio);

            await _tenantRepository.AdicionarAsync(tenant);

            // Cria Usuário Tenant Administrador
            var usuario = new Usuario
            {
                UserName = request.EmailAdmin,
                Email = request.EmailAdmin,
                TenantId = tenant.Id,
                NomeCompleto = "Administrador " + request.NomeLoja
            };

            var resultadoUsuario = await _userManager.CreateAsync(usuario, request.SenhaAdmin);

            if (!resultadoUsuario.Succeeded)
            {
                await _unitOfWork.RollbackAsync();
                var erros = string.Join(", ", resultadoUsuario.Errors.Select(e => e.Description));
                return new RegistrarLojistaResponse(false, null, $"Erro ao criar administrador: {erros}");
            }

            await _unitOfWork.CommitAsync();

            return new RegistrarLojistaResponse(true, tenant.Id);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new RegistrarLojistaResponse(false, null, $"Erro inesperado: {ex.Message}");
        }
    }
}