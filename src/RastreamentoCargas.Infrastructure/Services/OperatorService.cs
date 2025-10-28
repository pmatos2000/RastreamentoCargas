using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Application.DTOs.Operators;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.Infrastructure.Services
{
    public class OperatorService(AppDbContext context) : IOperatorService
    {
        public async Task<IEnumerable<OperatorResponseDto>> GetAllAsync()
        {
            var operators = await context.Operators
                .Where(o => o.IsActive)
                .ToListAsync();

            return operators.Select(o => (OperatorResponseDto) o);
        }

        public async Task<OperatorResponseDto?> GetOperator(Guid id)
        {
            var idString = id.ToString();

            var operatorEntity = await context.Operators
                .FirstOrDefaultAsync(o => o.Id == idString && o.IsActive);

            if (operatorEntity is null) return null;

            return (OperatorResponseDto)operatorEntity;
        }
    }
}
