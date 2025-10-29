using System.ComponentModel.DataAnnotations;

namespace RastreamentoCargas.Application.DTOs.Trips
{
    public record DeliverTripRequestDto
    {
        public string? FinalLocationDetails { get; init; }
    }
}
