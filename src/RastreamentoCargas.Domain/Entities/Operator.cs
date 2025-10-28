namespace RastreamentoCargas.Domain.Entities
{
    public sealed class Operator : User
    {
        public required string EmployeeId { get; set; }
        public required string Department { get; set; }
    }
}
