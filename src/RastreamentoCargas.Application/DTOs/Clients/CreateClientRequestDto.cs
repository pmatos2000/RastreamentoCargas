using RastreamentoCargas.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace RastreamentoCargas.Application.DTOs.Clients
{
    public record CreateClientRequestDto
    {
        [Required]
        public required string Name { get; init; }

        [Required]
        public required ClientDocumentType DocumentType { get; init; }

        [Required]
        public required string DocumentNumber { get; init; }

        [EmailAddress]
        public string? ContactEmail { get; init; }

        public string? Phone { get; init; }
    }
}
