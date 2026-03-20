using BROS.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace BROS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITenantUseCase, TenantUseCase>();

        return services;
    }
}