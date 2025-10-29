using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RastreamentoCargas.Domain.Common;
using RastreamentoCargas.Domain.Entities;

namespace RastreamentoCargas.Infrastructure.Data.Configurations
{
    public class TripConfiguration : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {

            builder.HasOne(t => t.Client) 
                   .WithMany(c => c.Trips) 
                   .HasForeignKey(t => t.ClientId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasOne(t => t.Operator) 
                   .WithMany(o => o.Trips) 
                   .HasForeignKey(t => t.OperatorId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired();

            builder.HasMany(t => t.History) 
                   .WithOne(th => th.Trip) 
                   .HasForeignKey(th => th.TripId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();

            builder.Property(t => t.OriginLocation)
                   .HasMaxLength(SchemaDefinition.LocationDefaultLength)
                   .IsRequired();

            builder.Property(t => t.DestinationLocation)
                   .HasMaxLength(SchemaDefinition.LocationDefaultLength)
                   .IsRequired();

            builder.Property(t => t.CurrentLocation)
                   .HasMaxLength(SchemaDefinition.LocationDefaultLength)
                   .IsRequired();
        }
    }
}