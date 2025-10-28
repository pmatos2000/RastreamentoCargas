using RastreamentoCargas.Domain.Interfaces.Common;

namespace RastreamentoCargas.Domain.Common
{
    public abstract class BaseEntity: IBaseEntity
    {
        public long Id { get; set; }
        public Guid ExternalId { get; set; }
    }
}
