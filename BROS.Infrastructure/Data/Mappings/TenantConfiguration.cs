using BROS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BROS.Infrastructure.Data.Mappings;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Subdominio)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(t => t.Subdominio)
            .IsUnique();

    }
}
