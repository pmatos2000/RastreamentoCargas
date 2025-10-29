using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Application.DTOs.Clients
{
    public record ClientResponseDto
    {
        public required long Id { get; init; }
        public required Guid ExternalId { get; init; }
        public required string Name { get; init; }
        public required ClientDocumentType DocumentType { get; set; }
        public required string Document { get; set; }
        public string? ContactEmail { get; init; }
        public string? Phone { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }

        public static explicit operator ClientResponseDto(Client client)
        {
            return new ClientResponseDto
            {
                Id = client.Id,
                ExternalId = client.ExternalId,
                Name = client.Name,
                DocumentType = client.DocumentType,
                Document = client.Document,
                ContactEmail = client.ContactEmail,
                Phone = client.Phone,
                IsActive = client.IsActive,
                CreatedAt = client.CreatedAt
            };
        }
    }
}
