using BROS.Domain.Entities;
using BROS.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BROS.Application.UseCases.Autenticacao.RealizarLogin;

public class RealizarLoginUseCase : IRealizarLoginUseCase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ITenantContext _tenantContext;

    public RealizarLoginUseCase(
        UserManager<Usuario> userManager,
        ITokenService tokenService,
        ITenantContext tenantContext)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _tenantContext = tenantContext;
    }

    public async Task<LoginResponse> ExecutarAsync(LoginRequest request)
    {
        if (_tenantContext.TenantId == null)
        {
            return new LoginResponse(false, null, "Contexto de Lojista não identificado.");
        }

        var usuario = await _userManager.FindByEmailAsync(request.Email);

        if (usuario == null)
        {
            return new LoginResponse(false, null, "E-mail ou senha inválidos.");
        }

        var senhaValida = await _userManager.CheckPasswordAsync(usuario, request.Senha);

        if (!senhaValida)
        {
            return new LoginResponse(false, null, "E-mail ou senha inválidos.");
        }

        var token = _tokenService.GerarToken(usuario);

        return new LoginResponse(true, token);
    }
}