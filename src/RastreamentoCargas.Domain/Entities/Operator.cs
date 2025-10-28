namespace RastreamentoCargas.Domain.Entities
{
    public class Operator : User
    {
        public required string EmployeeId { get; set; }
        public required string Department { get; set; }
    }
}
