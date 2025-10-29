using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Application.DTOs.TripHistoryRepositorys
{
    public record RegisterOccurrenceDto(
        long TripId,
        TripStatus Status,
        DateTime OccurrenceDateTime,
        string LocationDetails,
        string? Observation = null
    );
}
