using BROS.Application.DTOs;
using BROS.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BROS.Api.Controllers;

[ApiController]
[Route("api/v1/tenants")]
public class TenantsController : ControllerBase
{
    private readonly ITenantUseCase _tenantUseCase;

    public TenantsController(ITenantUseCase tenantUseCase)
    {
        _tenantUseCase = tenantUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] CreateTenantRequest request)
    {
        try
        {
            await _tenantUseCase.ExecuteAsync(request);

            return Ok(new { message = "Logista registrado com sucesso!"});
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = "Erro interno no servidor",
                details = ex.Message
            });
        }
    }
}
