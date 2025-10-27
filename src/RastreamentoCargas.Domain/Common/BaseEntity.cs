namespace RastreamentoCargas.Domain.Common
{
    public abstract class BaseEntity
    {
        public long Id { get; set; }
        public Guid ExternalId { get; set; }
    }
}
