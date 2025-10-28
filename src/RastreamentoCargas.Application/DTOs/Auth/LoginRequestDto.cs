using System.ComponentModel.DataAnnotations;

namespace RastreamentoCargas.Application.DTOs.Auth
{
    public record LoginRequestDto
    {
        [Required]
        public required string UserName { get; init; }
        [Required]
        public required string Password { get; init; }
    }
}
