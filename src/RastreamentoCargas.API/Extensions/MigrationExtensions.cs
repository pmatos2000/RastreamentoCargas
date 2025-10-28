using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.API.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyPendingMigrations(this WebApplication app)
        {
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
                    throw;
                }
            }
        }
    }
}