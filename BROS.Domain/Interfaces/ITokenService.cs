using BROS.Domain.Entities;

namespace BROS.Domain.Interfaces;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}