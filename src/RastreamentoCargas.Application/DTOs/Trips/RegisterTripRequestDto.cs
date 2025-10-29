using System.ComponentModel.DataAnnotations;


namespace RastreamentoCargas.Application.DTOs.Trips
{
    public record RegisterTripRequestDto
    {
        [Required(ErrorMessage = "O local de origem é obrigatório.")]
        public required string OriginLocation { get; init; }

        [Required(ErrorMessage = "O local de destino é obrigatório.")]
        public required string DestinationLocation { get; init; }

        [Required(ErrorMessage = "A data de saída é obrigatória.")]
        public required DateTime DepartureDate { get; init; }

        [Required(ErrorMessage = "A previsão de entrega é obrigatória.")]
        public required DateTime EstimatedDeliveryDate { get; init; }

        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        public required long ClientId { get; init; }
    }
}