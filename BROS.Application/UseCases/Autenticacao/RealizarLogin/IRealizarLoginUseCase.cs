namespace BROS.Application.UseCases.Autenticacao.RealizarLogin;

public record LoginRequest(string Email, string Senha);

public record LoginResponse(bool Sucesso, string? Token, string? Erro = null);

public interface IRealizarLoginUseCase
{
    Task<LoginResponse> ExecutarAsync(LoginRequest request);
}