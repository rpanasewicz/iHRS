using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class ReservationConfiguration : EntityBaseConfiguration<Reservation>
    {
        public override void ConfigureFields(EntityTypeBuilder<Reservation> entity)
        {
            entity.Property(r => r.StartDate).HasColumnType("datetime2(7)");
            entity.Property(r => r.EndDate).HasColumnType("datetime2(7)");
        }

        public override void ConfigureRelationships(EntityTypeBuilder<Reservation> entity)
        {
        }

        public override string TableName => "Reservations";
        public override string PrimaryKeyColumnName => "ReservationId";
    }
}
