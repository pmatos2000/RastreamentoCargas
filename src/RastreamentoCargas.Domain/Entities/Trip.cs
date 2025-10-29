using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Domain.Entities
{
    public class Trip : AuditableEntity
    {

        public required string OriginLocation { get; set; } 
        public required string DestinationLocation { get; set; } 
        public required DateTime DepartureDate { get; set; }  
        public required DateTime EstimatedDeliveryDate { get; set; } 
        public required string CurrentLocation { get; set; } 
        public required TripStatus CurrentStatus { get; set; } 

        public double OriginLatitude { get; set; }
        public double OriginLongitude { get; set; }
        public double DestinationLatitude { get; set; }
        public double DestinationLongitude { get; set; }
        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }

        public long ClientId { get; set; } 
        public virtual Client Client { get; set; } = null!;

        public long OperatorId { get; set; }
        public virtual Operator Operator { get; set; } = null!;

        public virtual ICollection<TripHistory> History { get; set; } = [];
    }
}