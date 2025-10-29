using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces.Repositories;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.Infrastructure.Repositories
{
    public class TripHistoryRepository(AppDbContext context) : ITripHistoryRepository
    {
        public async Task<TripHistory> CreateAsync(TripHistory historyEntry)
        {
            context.TripHistories.Add(historyEntry);
            await context.SaveChangesAsync();
            return historyEntry;
        }

        public async Task<IEnumerable<TripHistory>?> GetByTripCodeAsync(Guid tripCode)
        {
            var trip = await context.Trips
                .Include(t => t.History)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.ExternalId == tripCode);

            if (trip is null) return null;

            return trip.History.OrderByDescending(h => h.OccurrenceDateTime);
        }
    }
}
