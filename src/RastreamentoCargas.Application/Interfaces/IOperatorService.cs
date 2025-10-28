using RastreamentoCargas.Application.DTOs.Operators;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface IOperatorService
    {
        Task<IEnumerable<OperatorResponseDto>> GetAllAsync();
    }
}
