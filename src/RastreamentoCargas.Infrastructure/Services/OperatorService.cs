using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RastreamentoCargas.Application.DTOs.Operators;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Infrastructure.Data;

namespace RastreamentoCargas.Infrastructure.Services
{
    public sealed class OperatorService(AppDbContext context, UserManager<User> userManager) : IOperatorService
    {
        public async Task<IEnumerable<OperatorResponseDto>> GetAllAsync()
        {
            var operators = await context.Operators
                .Where(o => o.IsActive)
                .ToListAsync();

            return operators.Select(o => (OperatorResponseDto) o);
        }

        public async Task<OperatorResponseDto?> GetOperator(long id)
        {
            var operatorEntity = await context.Operators
                .FirstOrDefaultAsync(o => o.Id == id && o.IsActive);

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

        public async Task<bool> UpdateAsync(long id, UpdateOperatorRequestDto dto)
        {
            var operatorEntity = await context.Operators
                .FirstOrDefaultAsync(o => o.Id == id && o.IsActive);

            if (operatorEntity is null) return false;


            operatorEntity.Email = dto.Email ?? operatorEntity.Email;
            operatorEntity.FullName = dto.FullName ?? operatorEntity.FullName;
            operatorEntity.EmployeeId = dto.EmployeeId ?? operatorEntity.EmployeeId;
            operatorEntity.Department = dto.Department ?? operatorEntity.Department;
            operatorEntity.IsActive = dto.IsActive ?? operatorEntity.IsActive;

            context.Operators.Update(operatorEntity);
            await context.SaveChangesAsync();

            return true;
        }
    }
}
