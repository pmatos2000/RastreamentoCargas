using RastreamentoCargas.Application.DTOs.Common;
using RastreamentoCargas.Application.DTOs.TripHistories;
using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Domain.Common;



namespace RastreamentoCargas.Application.Interfaces
{
    public interface ITripHistoryService
    {
        Task RegisterOccurrenceAsync(RegisterOccurrenceDto dto);
        Task<IEnumerable<TripHistoryResponseDto>?> GetByTripCodeAsync(Guid tripCode);
        Task<PagedResponseDto<TripHistoryResponseDto>> GetAllAsync(SimplePaginationQuery query);
        Task AddManualHistoryAsync(Guid trackingCode, AddManualHistoryRequestDto dto);
    }
}
