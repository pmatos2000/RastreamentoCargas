using RastreamentoCargas.Application.DTOs.Common;
using RastreamentoCargas.Application.DTOs.TripHistories;
using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces.Repositories;

namespace RastreamentoCargas.Application.Services
{
    public sealed class TripHistoryService(ITripHistoryRepository tripHistoryRepository, ITripRepository tripRepository) : ITripHistoryService
    {
        public async Task RegisterOccurrenceAsync(RegisterOccurrenceDto dto)
        {
            var historyEntry = new TripHistory
            {
                TripId = dto.TripId,
                OccurrenceStatus = dto.Status,
                OccurrenceDateTime = dto.OccurrenceDateTime,
                LocationDetails = dto.LocationDetails,
                Observation = dto.Observation,
            };

            await tripHistoryRepository.CreateAsync(historyEntry);
        }

        public async Task<IEnumerable<TripHistoryResponseDto>?> GetByTripCodeAsync(Guid tripCode)
        {
            var histories = await tripHistoryRepository.GetByTripCodeAsync(tripCode);

            return histories?.Select(h => (TripHistoryResponseDto) h);
        }

        public async Task<PagedResponseDto<TripHistoryResponseDto>> GetAllAsync(SimplePaginationQuery query)
        {
            var pagedHistories = await tripHistoryRepository.GetAllAsync(query);

            return PagedResponseDto<TripHistoryResponseDto>
                .Create(pagedHistories, h => (TripHistoryResponseDto) h);
        }

        public async Task AddManualHistoryAsync(Guid trackingCode, AddManualHistoryRequestDto dto)
        {
            var trip = await tripRepository.GetByExternalIdAsync(trackingCode)
                ?? throw new InvalidOperationException($"Carga com código {trackingCode} não encontrada.");

            var occurrenceDto = new TripHistory
            {
                TripId = trip.Id,
                OccurrenceStatus = dto.Status,
                OccurrenceDateTime = dto.OccurrenceDateTime ?? DateTime.UtcNow,
                LocationDetails = dto.LocationDetails,
                Observation = dto.Observation,
            };

            await tripHistoryRepository.CreateAsync(occurrenceDto);
        }
    }
}
