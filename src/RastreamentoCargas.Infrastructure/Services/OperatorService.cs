using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Application.DTOs.Operators;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.Infrastructure.Services
{
    public class OperatorService(AppDbContext context, UserManager<User> userManager) : IOperatorService
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

        public async Task<OperatorResponseDto> CreateAsync(CreateOperatorRequestDto dto)
        {

            var newOperator = new Operator
            {
                UserName = dto.UserName,
                Email = dto.Email,
                FullName = dto.FullName,
                EmployeeId = dto.EmployeeId,
                Department = dto.Department,
                CreatedBy = string.Empty,
                CreatedAt = DateTime.UtcNow,
            };

            var result = await userManager.CreateAsync(newOperator, dto.Password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    "Falha ao criar operador: " +
                    string.Join(", ", result.Errors.Select(e => e.Description))
                );
            }

            return (OperatorResponseDto) newOperator;
        }
    }
}
