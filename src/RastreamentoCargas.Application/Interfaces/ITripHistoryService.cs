using RastreamentoCargas.Application.DTOs.TripHistoryRepositorys;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Application.Interfaces
{
    public interface ITripHistoryService
    {
        Task RegisterOccurrenceAsync(RegisterOccurrenceDto dto);
    }
}
