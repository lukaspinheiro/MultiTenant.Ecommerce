namespace BROS.Application.UseCases.Lojistas.RegistrarLojista;

public record RegistrarLojistaRequest(
    string NomeLoja, 
    string Subdominio, 
    string EmailAdmin, 
    string SenhaAdmin);

public record RegistrarLojistaResponse(bool Sucesso, Guid? TenantId, string? Erro = null);

public interface IRegistrarLojistaUseCase
{
    Task<RegistrarLojistaResponse> ExecutarAsync(RegistrarLojistaRequest request);
}