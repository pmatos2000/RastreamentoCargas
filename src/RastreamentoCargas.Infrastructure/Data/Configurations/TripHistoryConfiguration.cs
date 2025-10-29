using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Infrastructure.Data.Configurations
{
    public class TripHistoryConfiguration : IEntityTypeConfiguration<TripHistory>
    {
        public void Configure(EntityTypeBuilder<TripHistory> builder)
        {
            builder.Property(th => th.Observation)
                   .HasMaxLength(SchemaDefinition.ObservationDefaultLength);

            builder.Property(th => th.LocationDetails)
                   .HasMaxLength(SchemaDefinition.LocationDefaultLength )
                   .IsRequired();
        }
    }
}