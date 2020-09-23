using System.Security.Cryptography.X509Certificates;
using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class HotelConfiguration : BaseEntityConfiguration<Hotel>
    {
        public override void ConfigureFields(EntityTypeBuilder<Hotel> entity)
        {
            entity.Property(e => e.Name)
                .HasColumnType("nvarchar(500)")
                .HasMaxLength(250)
                .IsRequired();
        }

        public override void ConfigureRelationships(EntityTypeBuilder<Hotel> entity)
        {
        }

        public override string TableName => "Hotels";
        public override string PrimaryKeyColumnName => "HotelId";
    }
}
