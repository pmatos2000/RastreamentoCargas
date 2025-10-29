using System.ComponentModel.DataAnnotations;

namespace RastreamentoCargas.Application.DTOs.Clients
{
    public record UpdateClientRequestDto
    {
        public string? Name { get; init; }

        [EmailAddress]
        public string? ContactEmail { get; init; }

        public string? Phone { get; init; }
    }
}
