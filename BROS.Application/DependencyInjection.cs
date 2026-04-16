using BROS.Application.UseCases;
using BROS.Application.UseCases.Autenticacao.RealizarLogin;
using BROS.Application.UseCases.Lojistas.RegistrarLojista;
using Microsoft.Extensions.DependencyInjection;

namespace BROS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITenantUseCase, TenantUseCase>();
        services.AddScoped<IRealizarLoginUseCase, RealizarLoginUseCase>();
        services.AddScoped<IRegistrarLojistaUseCase, RegistrarLojistaUseCase>();

        return services;
    }
}