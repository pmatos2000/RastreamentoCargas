using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Application.DTOs.Clients;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.Infrastructure.Services
{
    public sealed class ClientService(IClientRepository clientRepository) : IClientService
    {
        public async Task<ClientResponseDto> CreateAsync(CreateClientRequestDto dto)
        {

            var newClient = new Client
            {
                Name = dto.Name,
                DocumentType = dto.DocumentType,
                Document = dto.DocumentNumber,
                ContactEmail = dto.ContactEmail,
                Phone = dto.Phone,
            };

            var client = await clientRepository.CreateAsync(newClient);

            return (ClientResponseDto) client;

        }

        public Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ClientResponseDto>> GetAllAsync()
        {
            var clients = await clientRepository.GetAllAsync();
            return clients.Select(c => (ClientResponseDto) c);
        }

        public async Task<ClientResponseDto?> GetByIdAsync(long id)
        {
           var client = await clientRepository.GetByIdAsync(id);

            if (client is null) return null;

            return  (ClientResponseDto) client;
        }

        public async Task<bool> UpdateAsync(long id, UpdateClientRequestDto dto)
        {
            var client = await clientRepository.GetByIdAsync(id);

            if (client is null) return false;

            client.Name = dto.Name ?? client.Name;
            client.ContactEmail = dto.ContactEmail ?? client.ContactEmail;
            client.Phone = dto.Phone ?? client.Phone;

            var result = await clientRepository.UpdateAsync(client);

            return result;
        }
    }
}
