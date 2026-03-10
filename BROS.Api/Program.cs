using BROS.Domain.Interfaces;
using BROS.Infrastructure.Data;
using BROS.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Configurações de Serviços de Infraestrutura
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
#endregion

#region Registro do EF Core com PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
#endregion

#region Registro dos Repositórios (se tiver mais, adicione aqui)
builder.Services.AddScoped<ITenantRepository, BROS.Infrastructure.Repositories.TenantRepository>();
#endregion

#region Serviços padrão
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
#endregion

#region Configuração do Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
#endregion