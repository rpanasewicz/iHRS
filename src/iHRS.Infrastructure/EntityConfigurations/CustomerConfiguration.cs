using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        public CustomerConfiguration(HRSContext context) : base(context)
        {
        }

        public override void ConfigureFields(EntityTypeBuilder<Customer> entity)
        {
            entity.Property(c => c.FirstName)
                .HasColumnType("nvarchar(250")
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(c => c.LastName)
                .HasColumnType("nvarchar(250)")
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(c => c.EmailAddress)
                .HasColumnType("nvarchar(250)")
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(c => c.PhoneNumber)
                .HasColumnType("nvarchar(15)")
                .HasMaxLength(15)
                .IsRequired();
        }

        public override void ConfigureRelationships(EntityTypeBuilder<Customer> entity)
        {
            entity.HasOne(c => c.Hotel)
                .WithMany(h => h.Customers)
                .HasForeignKey(c => c.HotelId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public override string TableName => "Customers";
        public override string PrimaryKeyColumnName => "CustomerId";
        public override IEnumerable<Customer> SeedData => null;
    }
}