using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Infrastructure.Services;

namespace RastreamentoCargas.API.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}