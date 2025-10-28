using RastreamentoCargas.Domain.Interfaces.Common;

namespace RastreamentoCargas.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity, IAuditable
    {
        public DateTime CreatedAt { get; set; }
        public required string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
