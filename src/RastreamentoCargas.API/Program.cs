using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders(); 

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            dbContext.Database.Migrate();
            logger.LogInformation("Migrações do banco de dados aplicadas com sucesso.");
        }
        else
        {
            logger.LogInformation("Banco de dados já está atualizado. Nenhuma migração aplicada.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Um erro ocorreu ao aplicar as migrações do banco de dados.");
        return;
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();


