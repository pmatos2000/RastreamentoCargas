using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Domain.Common;
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

        public async Task<PagedList<TripHistory>> GetAllAsync(SimplePaginationQuery paginationQuery)
        {
            var totalCount = await context.TripHistories.CountAsync();

            IQueryable<TripHistory> queryHistories = paginationQuery.SortAsc
                ? context.TripHistories
                    .AsNoTracking()
                    .OrderBy(h => h.OccurrenceDateTime)
                : context.TripHistories
                    .AsNoTracking()
                    .OrderByDescending(h => h.OccurrenceDateTime);
            
            queryHistories = queryHistories
                .Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
                .Take(paginationQuery.PageSize);

            var historie = await queryHistories.ToListAsync();

            return new PagedList<TripHistory>
            {
                Items = historie,
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize,
                TotalCount = totalCount,
            };
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
