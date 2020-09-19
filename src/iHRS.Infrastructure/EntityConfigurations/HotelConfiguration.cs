using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class HotelConfiguration : EntityBaseConfiguration<Hotel>
    {
        public override void ConfigureFields(EntityTypeBuilder<Hotel> entity)
        {
        }

        public override void ConfigureRelationships(EntityTypeBuilder<Hotel> entity)
        {
        }

        public override string TableName => "Hotels";
        public override string PrimaryKeyColumnName => "HotelId";
    }
}
