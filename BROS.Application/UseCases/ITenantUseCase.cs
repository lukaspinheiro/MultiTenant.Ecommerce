using BROS.Application.DTOs;

namespace BROS.Application.UseCases;

public interface ITenantUseCase
{
    Task ExecuteAsync(CreateTenantRequest request);
}
