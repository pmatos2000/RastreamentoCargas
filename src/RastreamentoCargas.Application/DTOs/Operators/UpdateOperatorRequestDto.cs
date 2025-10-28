using System.ComponentModel.DataAnnotations;

namespace RastreamentoCargas.Application.DTOs.Operators
{
    public record UpdateOperatorRequestDto
    {

        [EmailAddress]
        public string? Email { get; init; }
        public string? FullName { get; init; }

        public bool? IsActive { get; init; }

        public string? EmployeeId { get; init; }
        public string? Department { get; init; }
    }
}