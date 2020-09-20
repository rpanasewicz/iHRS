using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class CustomerConfiguration : EntityBaseConfiguration<Customer>
    {
        public override void ConfigureFields(EntityTypeBuilder<Customer> entity)
        {
        }

        public override void ConfigureRelationships(EntityTypeBuilder<Customer> entity)
        {
        }

        public override string TableName => "Customers";
        public override string PrimaryKeyColumnName => "CustomerId";
    }
}