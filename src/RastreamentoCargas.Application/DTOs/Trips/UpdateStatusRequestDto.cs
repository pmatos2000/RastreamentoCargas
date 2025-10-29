using RastreamentoCargas.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RastreamentoCargas.Application.DTOs.Trips
{
    public record UpdateStatusRequestDto
    {
        [Required(ErrorMessage = "O novo status é obrigatório.")]
        public required TripStatus NewStatus { get; init; }

        [Required(ErrorMessage = "A nova localização é obrigatória.")]
        public required string NewLocation { get; init; }

        public string? Observation { get; init; }
    }
}
