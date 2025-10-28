using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Entities;


namespace RastreamentoCargas.Infrastructure.Data.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(o => o.Document)
                .HasMaxLength(SchemaDefinition.DoumentDefaultLength);

            builder.Property(o => o.ContactEmail)
                .HasMaxLength(SchemaDefinition.EmailDefaultLength);

            builder.Property(o => o.Phone)
                .HasMaxLength(SchemaDefinition.PhoneDefaultLength);
        }
    }
}
