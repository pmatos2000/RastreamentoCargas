using Microsoft.AspNetCore.Identity;
using RastreamentoCargas.Domain.Interfaces.Common;

namespace RastreamentoCargas.Domain.Entities
{
    public class User : IdentityUser<long>, IAuditable
    {
        public required string FullName { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = default!;
        public string CreatedBy { get; set; } = null!;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public Guid ExternalId { get; set; }
    }
}
