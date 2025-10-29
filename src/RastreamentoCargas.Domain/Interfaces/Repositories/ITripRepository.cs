using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Domain.Interfaces.Repositories
{
    public interface ITripRepository
    {
        Task<Trip> CreateAsync(Trip trip);
        Task<IEnumerable<Trip>> GetAllAsync();
        Task<Trip?> GetByExternalIdAsync(Guid externalId);
    }
}
