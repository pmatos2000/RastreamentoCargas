using FluentValidation;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Application.Validators.Client;
using RastreamentoCargas.Domain.Interfaces.Repositories;
using RastreamentoCargas.Infrastructure.Repositories;
using RastreamentoCargas.Infrastructure.Services;

namespace RastreamentoCargas.API.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IOperatorService, OperatorService>();

            services.AddScoped<IClientRepository, ClientRepository>();

            services.AddValidatorsFromAssemblyContaining<CreateClientRequestDtoValidator>();

            return services;
        }
    }
}