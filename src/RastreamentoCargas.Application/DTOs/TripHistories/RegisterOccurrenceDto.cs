using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Application.DTOs.TripHistories
{
    public record RegisterOccurrenceDto(
        long TripId,
        TripStatus Status,
        DateTime OccurrenceDateTime,
        string LocationDetails,
        string? Observation = null
    );
}
