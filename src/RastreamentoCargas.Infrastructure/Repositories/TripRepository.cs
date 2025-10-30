using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Enums;
using RastreamentoCargas.Domain.Interfaces.Repositories;
using RastreamentoCargas.Infrastructure.Data;


namespace RastreamentoCargas.Infrastructure.Repositories
{
    public sealed class TripRepository(AppDbContext context) : ITripRepository
    {
        private IQueryable<Trip> GetFullQuery() => context.Trips
            .Include(t => t.Client)
            .Include(t => t.Operator);

        public async Task<Trip> CreateAsync(Trip trip)
        {
            context.Trips.Add(trip);
            await context.SaveChangesAsync();
            return trip;
        }

        public async Task<IEnumerable<Trip>> GetAllAsync()
        {
            return await GetFullQuery().Where(t => t.IsActive).ToListAsync();
        }

        public async Task<Trip?> GetByExternalIdAsync(Guid externalId)
        {
            return await GetFullQuery()
                .FirstOrDefaultAsync(t => t.ExternalId == externalId && t.IsActive);
        }

        public async Task<bool> UpdateAsync(Trip trip)
        {
            context.Trips.Update(trip);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Trip>> GetByStatusAsync(TripStatus status)
        {
            return await GetFullQuery()
                .Where(t => t.CurrentStatus == status && t.IsActive)
                .ToListAsync();
        }

        public async Task<bool> HasActiveTripsByClientIdAsync(long clientId)
        {
            return await context.Trips
                .AnyAsync(t => t.ClientId == clientId &&
                               t.CurrentStatus != TripStatus.Delivered &&
                               t.CurrentStatus != TripStatus.Canceled &&
                               t.IsActive);
        }

        public async Task<bool> HasActiveTripsByOperatorIdAsync(long operatorId)
        {
            return await context.Trips
                .AnyAsync(t => t.OperatorId == operatorId &&
                               t.CurrentStatus != TripStatus.Delivered &&
                               t.CurrentStatus != TripStatus.Canceled &&
                               t.IsActive);
        }
    }
}