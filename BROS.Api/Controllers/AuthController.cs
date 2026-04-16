using BROS.Application.UseCases.Autenticacao.RealizarLogin;
using BROS.Application.UseCases.Lojistas.RegistrarLojista;
using Microsoft.AspNetCore.Mvc;

namespace BROS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRealizarLoginUseCase _loginUseCase;
    private readonly IRegistrarLojistaUseCase _registrarLojistaUseCase;

    public AuthController(
        IRealizarLoginUseCase loginUseCase,
        IRegistrarLojistaUseCase registrarLojistaUseCase)
    {
        _loginUseCase = loginUseCase;
        _registrarLojistaUseCase = registrarLojistaUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var resultado = await _loginUseCase.ExecutarAsync(request);

        if (!resultado.Sucesso)
        {
            return Unauthorized(new { erro = resultado.Erro });
        }

        return Ok(new { token = resultado.Token });
    }

    [HttpPost("registrar-lojista")]
    public async Task<IActionResult> RegistrarLojista([FromBody] RegistrarLojistaRequest request)
    {
        var resultado = await _registrarLojistaUseCase.ExecutarAsync(request);

        if (!resultado.Sucesso)
        {
            return BadRequest(new { erro = resultado.Erro });
        }

        return Created($"/api/tenants/{resultado.TenantId}", new { tenantId = resultado.TenantId });
    }
}