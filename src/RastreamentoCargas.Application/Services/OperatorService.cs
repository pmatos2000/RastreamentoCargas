using Microsoft.AspNetCore.Identity;
using RastreamentoCargas.Application.DTOs.Operators;
using RastreamentoCargas.Application.Interfaces;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Interfaces.Repositories;

namespace RastreamentoCargas.Application.Services
{
    public sealed class OperatorService(
        IOperatorRepository operatorRepository,
        ITripRepository tripRepository,
        UserManager<User> userManager) : IOperatorService
    {
        public async Task<IEnumerable<OperatorResponseDto>> GetAllAsync()
        {
            var operators = await operatorRepository.GetAllAsync();
            return operators.Select(o => (OperatorResponseDto) o);
        }

        public async Task<OperatorResponseDto?> GetByIdAsync(long id)
        {
            var operatorEntity = await operatorRepository.GetByIdAsync(id);
            
            if(operatorEntity is null) return null;

            return (OperatorResponseDto) operatorEntity;
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
            var operatorEntity = await operatorRepository.GetByIdAsync(id);

            if (operatorEntity is null) return false;

            operatorEntity.Email = dto.Email ?? operatorEntity.Email;
            operatorEntity.FullName = dto.FullName ?? operatorEntity.FullName;
            operatorEntity.EmployeeId = dto.EmployeeId ?? operatorEntity.EmployeeId;
            operatorEntity.Department = dto.Department ?? operatorEntity.Department;

            return await operatorRepository.UpdateAsync(operatorEntity); ;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var operatorEntity = await operatorRepository.GetByIdAsync(id);
            if (operatorEntity == null) return false;

            // TODO: Criar regra para um operator não conseguir desativar um operador admin

            if(await tripRepository.HasActiveTripsByOperatorIdAsync(id))
                throw new InvalidOperationException("Não é possível desativar o operador. Existem cargas ativas (não entregues ou canceladas) associadas a ele.");
            
            return await operatorRepository.DeleteAsync(id);
        }
    }
}
