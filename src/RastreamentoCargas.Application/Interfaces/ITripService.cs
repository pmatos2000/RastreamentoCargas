using RastreamentoCargas.Application.DTOs.Trips;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface ITripService
    {
        Task<TripResponseDto> RegisterAsync(RegisterTripRequestDto dto, long operatorId);

        Task<IEnumerable<TripResponseDto>> GetAllAsync();

        Task<TripResponseDto?> GetByCodeAsync(Guid codigoCarga);
    }
}