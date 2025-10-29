using RastreamentoCargas.Domain.Interfaces.Common;

namespace RastreamentoCargas.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity, IAuditable
    {
        public DateTime CreatedAt { get; set; } = default!;
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
