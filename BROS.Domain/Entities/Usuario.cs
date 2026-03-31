using Microsoft.AspNetCore.Identity;
using BROS.Domain.Interfaces;

namespace BROS.Domain.Entities;

public class Usuario : IdentityUser<Guid>, ITenantEntity
{
    public Guid? TenantId { get; set; }

    public string NomeCompleto { get; set; } = string.Empty;
    Guid ITenantEntity.TenantId
    {
        get => TenantId ?? Guid.Empty;
        set => TenantId = value == Guid.Empty ? null : value;
    }
}