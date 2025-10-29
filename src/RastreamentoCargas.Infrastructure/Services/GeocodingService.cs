using RastreamentoCargas.Application.DTOs.Coordinates;
using RastreamentoCargas.Application.Interfaces;

namespace RastreamentoCargas.Infrastructure.Services
{
    public sealed class GeocodingService : IGeocodingService
    {
        public Task<CoordinatesDto> GetCoordinatesAsync(string address)
        {
            if (address.Contains("São Paulo"))
            {
                return Task.FromResult(new CoordinatesDto(-23.5505, -46.6333));
            }
            if (address.Contains("Rio de Janeiro"))
            {
                return Task.FromResult(new CoordinatesDto(-22.9068, -43.1729));
            }

            return Task.FromResult(new CoordinatesDto(-15.7801, -47.9292));
        }
    }
}