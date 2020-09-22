using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        public override void ConfigureFields(EntityTypeBuilder<Customer> entity)
        {
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
    }
}