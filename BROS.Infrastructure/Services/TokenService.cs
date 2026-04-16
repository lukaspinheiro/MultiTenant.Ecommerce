using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BROS.Domain.Entities;
using BROS.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BROS.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GerarToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"] ?? "ChaveMestraSecretaParaDesenvolvimento12345678");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Email ?? string.Empty),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim("tenant_id", usuario.TenantId?.ToString() ?? Guid.Empty.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}