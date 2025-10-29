using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces.Repositories;
using RastreamentoCargas.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RastreamentoCargas.Infrastructure.Repositories
{
    public sealed class ClientRepository(AppDbContext context) : IClientRepository
    {
        public async Task<Client> CreateAsync(Client client)
        {
            context.Clients.Add(client);
            await context.SaveChangesAsync();
            return client;
        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            var clients = await context.Clients
                .Where(c => c.IsActive)
                .ToListAsync();

            return clients;
        }

        public async Task<Client?> GetByIdAsync(long id)
        {
            var client = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            return client;
        }

        public async Task<bool> IsDocumentUnique(string document)
        {
            var exists =  !await context.Clients
                .AnyAsync(c => c.Document == document && c.IsActive);

            return !exists;
        }

        public async Task<bool> UpdateAsync(Client client)
        {
            context.Clients.Update(client);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
