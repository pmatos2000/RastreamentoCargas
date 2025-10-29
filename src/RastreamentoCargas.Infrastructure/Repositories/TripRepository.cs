using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces;
using RastreamentoCargas.Domain.Interfaces.Repositories;
using RastreamentoCargas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}