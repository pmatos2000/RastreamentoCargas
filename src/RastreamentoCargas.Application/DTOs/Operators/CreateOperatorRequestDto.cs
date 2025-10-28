using RastreamentoCargas.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace RastreamentoCargas.Application.DTOs.Operators
{
    public record CreateOperatorRequestDto
    {

        [Required]
        [MaxLength(SchemaDefinition.User.UserNameLength)]
        public required string UserName { get; init; }
        
        [EmailAddress]
        public string? Email { get; init; }

        [Required]
        [MaxLength(SchemaDefinition.User.UserNameLength)]
        public required string FullName { get; init; }

        [Required]
        [MinLength(6)]
        public required string Password { get; init; }

        [Required]
        [MaxLength(SchemaDefinition.Operator.EmployeeIdLength)]
        public required string EmployeeId { get; init; }

        [Required]
        [MaxLength(SchemaDefinition.Operator.DepartmentNameLength)]
        public required string Department { get; init; }
    }
}
