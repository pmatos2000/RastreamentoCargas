using RastreamentoCargas.Application.DTOs.Operators;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface IOperatorService
    {
        Task<IEnumerable<OperatorResponseDto>> GetAllAsync();
        Task<OperatorResponseDto?> GetByIdAsync(long id);
        Task<OperatorResponseDto> CreateAsync(CreateOperatorRequestDto dto);
        Task<bool> UpdateAsync(long id, UpdateOperatorRequestDto dto);
    }
}
