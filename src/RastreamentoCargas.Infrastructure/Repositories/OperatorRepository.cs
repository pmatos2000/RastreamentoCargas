using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces.Repositories;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.Infrastructure.Repositories
{
    public sealed class OperatorRepository(AppDbContext context) : IOperatorRepository
    {
        public async Task<bool> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Operator>> GetAllAsync()
        {
            var operators = await context.Operators
                .Where(c => c.IsActive)
                .ToListAsync();

            return operators;
        }

        public async Task<Operator?> GetByIdAsync(long id)
        {
            var operatorEntity = await context.Operators
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            return operatorEntity;
        }

        public async Task<bool> UpdateAsync(Operator operatorEntity)
        {
            context.Operators.Update(operatorEntity);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
