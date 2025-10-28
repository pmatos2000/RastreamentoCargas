using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Domain.Entities
{
    public sealed class Client : AuditableEntity
    {
        public required string Name { get; set; }
        public required ClientDocumentType DocumentType { get; set; }
        public required string Document { get; set; }
        public string? ContactEmail { get; set; }
        public string? Phone { get; set; }
    }
}
