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
        public override IEnumerable<Hotel> SeedData => null;
    }
}
