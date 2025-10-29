using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces.Repositories;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.Infrastructure.Repositories
{
    internal class TripHistoryRepository(AppDbContext context) : ITripHistoryRepository
    {
        public async Task<TripHistory> CreateAsync(TripHistory historyEntry)
        {
            context.TripHistories.Add(historyEntry);
            await context.SaveChangesAsync();
            return historyEntry;
        }
    }
}
