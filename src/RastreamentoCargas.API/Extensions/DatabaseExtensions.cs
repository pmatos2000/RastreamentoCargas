using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection AddDatabaseAndIdentity(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));


            services.AddIdentity<User, IdentityRole<long>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}