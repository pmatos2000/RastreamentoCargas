using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Enums;
using RastreamentoCargas.Domain.Interfaces.Repositories;

namespace RastreamentoCargas.Application.Services
{
    public sealed class TripHistoryService(ITripHistoryRepository tripHistoryRepository) : ITripHistoryService
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
    }
}
