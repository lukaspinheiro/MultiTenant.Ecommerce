using BROS.Application; 
using BROS.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

#region Serviços padrão
builder.Services.AddControllers();
builder.Services.AddOpenApi();
#endregion

var app = builder.Build();

#region Configuração do Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/openapi/v1.json", "BROS.Api v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseMiddleware<BROS.Api.Middlewares.TenantResolutionMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
#endregion