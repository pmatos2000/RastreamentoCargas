using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Domain.Entities
{
    public class TripHistory : BaseEntity
    {
        public required TripStatus OccurrenceStatus { get; set; }
        public required DateTime OccurrenceDateTime { get; set; }
        public required string LocationDetails { get; set; }
        public string? Observation { get; set; }

        public long TripId { get; set; }
        public virtual required Trip Trip { get; set; } = null!;
    }

}
