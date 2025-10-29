using RastreamentoCargas.Application.DTOs.Clients;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientResponseDto>> GetAllAsync();
        Task<ClientResponseDto?> GetByIdAsync(long id);
        Task<ClientResponseDto> CreateAsync(CreateClientRequestDto dto);
        Task<bool> UpdateAsync(long id, UpdateClientRequestDto dto);
        Task<bool> DeleteAsync(long id);
    }
}
