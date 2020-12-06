using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class HotelConfiguration : BaseEntityConfiguration<Hotel>
    {
        public HotelConfiguration(HRSContext context) : base(context)
        {
        }

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
        public override IEnumerable<Hotel> SeedData
        {
            get
            {
                var hotel = Hotel.CreateNew("My first hotel");
                var hotel2 = Hotel.CreateNew("My second hotel");

                typeof(Hotel)
                    .GetProperty("Id")
                    .ForceSetValue(hotel, new Guid("00000000-0000-0000-0000-000000000004"))
                    .ForceSetValue(hotel2, new Guid("00000000-0000-0000-0000-000000000005"));

                return new Hotel[] { hotel, hotel2 };
            }
        }
    }
}
