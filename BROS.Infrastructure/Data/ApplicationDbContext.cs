using BROS.Domain.Entities;
using BROS.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace BROS.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<Usuario, IdentityRole<Guid>, Guid>
{
    private readonly ITenantProvider _tenantProvider;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IServiceProvider? serviceProvider = null) 
        : base(options)
    {
        _tenantProvider = serviceProvider?.GetService<ITenantProvider>()!;
    }

    public DbSet<Tenant> Tenants { get; set; } = default!;

    public Guid? CurrentTenantId => _tenantProvider?.ObterTenantId();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => new { u.NormalizedUserName, u.TenantId })
                  .IsUnique()
                  .HasDatabaseName("MultiTenantUserNameIndex");

            entity.HasIndex(u => new { u.NormalizedEmail, u.TenantId })
                  .IsUnique()
                  .HasDatabaseName("MultiTenantEmailIndex");
        });

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ITenantEntity).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                
                var property = Expression.Property(parameter, nameof(ITenantEntity.TenantId));

                var contextProperty = Expression.Property(Expression.Constant(this), nameof(CurrentTenantId));

                Expression comparison = Expression.Equal(property, Expression.Convert(contextProperty, property.Type));

                var lambda = Expression.Lambda(comparison, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }
}
