using RastreamentoCargas.Application.DTOs.Operators;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface IOperatorService
    {
        Task<IEnumerable<OperatorResponseDto>> GetAllAsync();
        Task<OperatorResponseDto?> GetOperator(long id);
        Task<OperatorResponseDto> CreateAsync(CreateOperatorRequestDto dto);
    }
}
