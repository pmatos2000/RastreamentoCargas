using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RastreamentoCargas.Domain.Entities;
using RastreamentoCargas.Domain.Common; 

namespace RastreamentoCargas.Infrastructure.Data.Configurations
{
    public class OperatorConfiguration : IEntityTypeConfiguration<Operator>
    {
        public void Configure(EntityTypeBuilder<Operator> builder)
        {
            builder.Property(o => o.EmployeeId)
                .HasMaxLength(SchemaDefinition.Operator.EmployeeIdLength);

            builder.Property(o => o.Department)
                .HasMaxLength(SchemaDefinition.Operator.DepartmentNameLength);
        }
    }
}