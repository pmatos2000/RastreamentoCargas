using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client?> GetByIdAsync(long id);
        Task<Client> CreateAsync(Client client);
        Task<bool> UpdateAsync(Client clieent);
        Task<bool> DeleteAsync(long id);
        Task<bool> IsDocumentUnique(string document);
    }
}