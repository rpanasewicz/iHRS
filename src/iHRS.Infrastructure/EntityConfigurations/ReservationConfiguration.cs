using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class ReservationConfiguration : BaseEntityConfiguration<Reservation>
    {
        public ReservationConfiguration(Guid tenantId) : base(tenantId)
        {
        }

        public override void ConfigureFields(EntityTypeBuilder<Reservation> entity)
        {
            entity.Ignore(e => e.Status);

            entity
                .Property(r => r.StartDate)
                .HasColumnType("datetime2(7)")
                .IsRequired();

            entity
                .Property(r => r.EndDate)
                .HasColumnType("datetime2(7)")
                .IsRequired();

            entity
                .Property(e => e.NumberOfPersons)
                .HasColumnType("int")
                .IsRequired();
        }

        public override void ConfigureRelationships(EntityTypeBuilder<Reservation> entity)
        {
            entity.HasOne(r => r.Room)
                .WithMany(h => h.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.NoAction);

            entity.HasOne(r => r.Customer)
                .WithMany(h => h.Reservations)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public override string TableName => "Reservations";
        public override string PrimaryKeyColumnName => "ReservationId";
    }
}
