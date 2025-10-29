using RastreamentoCargas.Application.DTOs.Coordinates;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface IGeocodingService
    {
        Task<CoordinatesDto> GetCoordinatesAsync(string address);
    }
}
