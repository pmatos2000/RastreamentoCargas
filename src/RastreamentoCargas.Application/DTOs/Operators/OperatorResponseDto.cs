using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Application.DTOs.Operators
{
    public record OperatorResponseDto
    {
        public required string Id { get; init; }
        public required string UserName { get; init; }
        public string? Email { get; init; }
        public required string FullName { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
        public required string EmployeeId { get; init; }
        public required string Department { get; init; }

        public static explicit operator OperatorResponseDto(Operator op)
        {
            // A mesma lógica do seu método
            return new OperatorResponseDto
            {
                Id = op.Id,
                UserName = op.UserName ?? "",
                Email = op.Email,
                FullName = op.FullName,
                IsActive = op.IsActive,
                CreatedAt = op.CreatedAt,
                EmployeeId = op.EmployeeId,
                Department = op.Department
            };
        }
    }
}
