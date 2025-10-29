using FluentValidation;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Application.Services;
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
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<ITripHistoryService, TripHistoryService>();
            services.AddScoped<ITripService, TripService>();
            services.AddScoped<IGeocodingService, GeocodingService>();

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IOperatorRepository, OperatorRepository>();
            services.AddScoped<ITripRepository, TripRepository>();
            services.AddScoped<ITripHistoryRepository, TripHistoryRepository>();

            services.AddValidatorsFromAssemblyContaining<CreateClientRequestDtoValidator>();

            services.AddHttpClient<IGeocodingService, GeocodingService>(client =>
            {
                client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("RastreamentoCargasAPI/1.0 (pmatos2000@gmail.com)");
            });

            return services;
        }
    }
}