using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Enums;
using RastreamentoCargas.Domain.Extensions; 

namespace RastreamentoCargas.Application.DTOs.TripHistoryRepositorys
{
    public record TripHistoryResponseDto
    {
        public Guid Id { get; init; }
        public required TripStatus Status { get; init; }
        public required string StatusDescription { get; init; }
        public DateTime OccurrenceDateTime { get; init; }
        public required string LocationDetails { get; init; }
        public string? Observation { get; init; }

        public static explicit operator TripHistoryResponseDto(TripHistory history)
        {
            return new TripHistoryResponseDto
            {
                Id = history.ExternalId,
                Status = history.OccurrenceStatus,
                StatusDescription = history.OccurrenceStatus.GetFriendlyName(),
                OccurrenceDateTime = history.OccurrenceDateTime,
                LocationDetails = history.LocationDetails,
                Observation = history.Observation
            };
        }
    }
}