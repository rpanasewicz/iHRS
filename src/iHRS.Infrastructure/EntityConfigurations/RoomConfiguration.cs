using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class RoomConfiguration : BaseEntityConfiguration<Room>
    {
        public override void ConfigureFields(EntityTypeBuilder<Room> entity)
        {
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