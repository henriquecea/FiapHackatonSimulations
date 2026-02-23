using FiapHackatonSimulations.Application.Service;
using FiapHackatonSimulations.Domain.Interface.Repository;
using FiapHackatonSimulations.Domain.Interface.Service;
using FiapHackatonSimulations.Infrastructure.Data;
using FiapHackatonSimulations.Infrastructure.Repository;
using FiapHackatonSimulations.WebAPI.Extension;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Serviços customizados
builder.AddSwagger();
builder.AddDbContext();
builder.AddRabbitMQ();

// Serviços padrão ASP.NET
builder.Services.AddAuthorization();
builder.Services.AddControllers()
                .AddNewtonsoftJson();

// Injeção de dependência
builder.Services.AddScoped<ISimulationService, SimulationService>();

builder.Services.AddScoped<ISimulationRepository, SimulationRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Aplicar migrações pendentes ao iniciar a aplicações
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

// Redirecionamento da raiz para /swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
