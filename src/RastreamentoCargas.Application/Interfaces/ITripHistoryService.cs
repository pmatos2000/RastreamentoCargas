using RastreamentoCargas.Application.DTOs.TripHistories;
using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Domain.Entities;


namespace RastreamentoCargas.Application.Interfaces
{
    public interface ITripHistoryService
    {
        Task RegisterOccurrenceAsync(RegisterOccurrenceDto dto);
        Task<IEnumerable<TripHistoryResponseDto>?> GetByTripCodeAsync(Guid tripCode);
    }
}
