using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Domain.Interfaces.Repositories
{
    public interface ITripRepository
    {
        Task<Trip> CreateAsync(Trip trip);
        Task<IEnumerable<Trip>> GetAllAsync();
        Task<Trip?> GetByExternalIdAsync(Guid externalId);
        Task<bool> UpdateAsync(Trip trip);
        Task<IEnumerable<Trip>> GetByStatusAsync(TripStatus status);
        Task<bool> HasActiveTripsByClientIdAsync(long clientId);
    }
}
