using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Application.DTOs.Clients;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.Infrastructure.Services
{
    public sealed class ClientService(AppDbContext context) : IClientService
    {
        public async Task<ClientResponseDto> CreateAsync(CreateClientRequestDto dto)
        {

            var newClient = new Domain.Entities.Client
            {
                Name = dto.Name,
                DocumentType = dto.DocumentType,
                Document = dto.DocumentNumber,
                ContactEmail = dto.ContactEmail,
                Phone = dto.Phone,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = string.Empty,
            };

            context.Clients.Add(newClient);

            await context.SaveChangesAsync();

            return (ClientResponseDto) newClient;

        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ClientResponseDto>> GetAllAsync()
        {
            var clients = await context.Clients
                .Where(c => c.IsActive)
                .ToListAsync();

            return clients.Select(c => (ClientResponseDto) c);
        }

        public async Task<ClientResponseDto?> GetByIdAsync(long id)
        {
           var client = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (client is null) return null;

            return  (ClientResponseDto) client;
        }

        public async Task<bool> IsDocumentUnique(string document)
        {
            return !await context.Clients
                .AnyAsync(c => c.Document == document && c.IsActive);
        }

        public async Task<bool> UpdateAsync(long id, UpdateClientRequestDto dto)
        {
            var client = await context.Clients
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if(client is null) return false;

            client.Name = dto.Name ?? client.Name;
            client.ContactEmail = dto.ContactEmail ?? client.ContactEmail; 
            client.Phone = dto.Phone ?? client.Phone;

            return true;
        }
    }
}
