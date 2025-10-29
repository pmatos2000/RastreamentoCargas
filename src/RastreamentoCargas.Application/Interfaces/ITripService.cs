using RastreamentoCargas.Application.DTOs.Trips;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface ITripService
    {
        Task<TripResponseDto> RegisterAsync(RegisterTripRequestDto dto, long operatorId);

        Task<IEnumerable<TripResponseDto>> GetAllAsync();

        Task<TripResponseDto?> GetByCodeAsync(Guid codigoCarga);

        Task<TripResponseDto> UpdateStatusAsync(Guid trackingCode, UpdateStatusRequestDto dto);

        Task<TripResponseDto> UpdateLocationAsync(Guid trackingCode, UpdateLocationRequestDto dto);

        Task<TripResponseDto> DeliverTripAsync(Guid trackingCode, string? finalLocationDetails);
        Task<bool> CancelAsync(Guid trackingCode);
    }
}