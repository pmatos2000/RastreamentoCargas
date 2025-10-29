using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Domain.Interfaces.Repositories
{
    public interface IOperatorRepository
    {
        Task<IEnumerable<Operator>> GetAllAsync();
        Task<Operator?> GetByIdAsync(long id);
        Task<bool> UpdateAsync(Operator operatorEntity);
        Task<bool> DeleteAsync(long id);
    }
}
