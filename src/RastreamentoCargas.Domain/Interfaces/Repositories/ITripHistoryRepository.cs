using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Domain.Interfaces.Repositories
{
    public interface ITripHistoryRepository
    {
        Task<TripHistory> CreateAsync(TripHistory historyEntry);
        Task<IEnumerable<TripHistory>?> GetByTripCodeAsync(Guid tripCode);
    }
}
