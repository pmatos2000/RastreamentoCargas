using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Domain.Entities
{
    public class Client : AuditableEntity
    {
        public required string Name { get; set; }
        public required ClientDocumentType DocumentType { get; set; }
        public required string Document { get; set; }
        public string? ContactEmail { get; set; }
        public string? Phone { get; set; }


        public virtual ICollection<Trip> Trips { get; set; } = [];
    }
}
