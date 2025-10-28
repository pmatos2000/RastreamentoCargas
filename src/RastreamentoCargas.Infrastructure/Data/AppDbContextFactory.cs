using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RastreamentoCargas.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            var connectionString = "Server=localhost,1433;Database=RastreamentoCargasDb;User Id=sa;Password=Password@123;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(connectionString);
            var httpContextAccessor = new HttpContextAccessor();

            return new AppDbContext(optionsBuilder.Options, httpContextAccessor);
        }
    }
}