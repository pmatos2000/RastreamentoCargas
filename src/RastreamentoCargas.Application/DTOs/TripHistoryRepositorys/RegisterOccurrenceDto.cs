using RastreamentoCargas.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
