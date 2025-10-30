using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Domain.Interfaces.Repositories
{
    public interface ITripHistoryRepository
    {
        Task<TripHistory> CreateAsync(TripHistory historyEntry);
        Task<IEnumerable<TripHistory>?> GetByTripCodeAsync(Guid tripCode);
        Task<PagedList<TripHistory>> GetAllAsync(SimplePaginationQuery query);
    }
}
