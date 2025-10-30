using FluentValidation;
using RastreamentoCargas.Application.DTOs.Clients;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces.Repositories;

namespace RastreamentoCargas.Application.Services
{
    public sealed class ClientService(
        IClientRepository clientRepository,
        ITripRepository tripRepository,
        IValidator<CreateClientRequestDto> validatorCreateClientRequestDto) : IClientService
    {
        public async Task<ClientResponseDto> CreateAsync(CreateClientRequestDto dto)
        {
            var validationResult = await validatorCreateClientRequestDto.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var newClient = new Client
            {
                Name = dto.Name,
                DocumentType = dto.DocumentType,
                Document = dto.Document,
                ContactEmail = dto.ContactEmail,
                Phone = dto.Phone,
            };

            var client = await clientRepository.CreateAsync(newClient);

            return (ClientResponseDto) client;

        }

        public async Task<bool> DeleteAsync(long id)
        {
            var client = await clientRepository.GetByIdAsync(id);
            if (client is null) return false;


            bool hasActiveTrips = await tripRepository.HasActiveTripsByClientIdAsync(id);
            if (hasActiveTrips)
            {
                throw new InvalidOperationException("Não é possível desativar o cliente. Existem cargas ativas associadas a ele.");
            }

            return await clientRepository.DeleteAsync(id);
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
