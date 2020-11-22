using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class RoomConfiguration : BaseEntityConfiguration<Room>
    {
        public RoomConfiguration(Guid tenantId) : base(tenantId)
        {
        }

        public override void ConfigureFields(EntityTypeBuilder<Room> entity)
        {
            entity.Property(e => e.RoomNumber)
                .HasColumnType("nvarchar(10)")
                .HasMaxLength(10)
                .IsRequired();
        }

        public override void ConfigureRelationships(EntityTypeBuilder<Room> entity)
        {
            entity.HasOne(r => r.Hotel)
                .WithMany(h => h.Rooms)
                .HasForeignKey(r => r.HotelId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public override string TableName => "Rooms";
        public override string PrimaryKeyColumnName => "RoomId";
    }
}