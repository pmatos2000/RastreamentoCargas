using RastreamentoCargas.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RastreamentoCargas.Application.DTOs.TripHistories
{

    public record AddManualHistoryRequestDto
    {
        [Required(ErrorMessage = "O status da ocorrência é obrigatório.")]
        public required TripStatus Status { get; init; }

        [Required(ErrorMessage = "Os detalhes da localização são obrigatórios.")]
        public required string LocationDetails { get; init; }

        public required DateTime? OccurrenceDateTime { get; init; }

        public string? Observation { get; init; }
    }
}