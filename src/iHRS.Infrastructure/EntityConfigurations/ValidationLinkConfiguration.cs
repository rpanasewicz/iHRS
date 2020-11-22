using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class ValidationLinkConfiguration : BaseEntityConfiguration<ValidationLink>
    {
        public ValidationLinkConfiguration(Guid tenantId) : base(tenantId)
        {
        }

        public override string TableName => "ValidationLinks";
        public override string PrimaryKeyColumnName => "ValidationLinkId";
        public override IEnumerable<ValidationLink> SeedData => null;

        public override void ConfigureFields(EntityTypeBuilder<ValidationLink> entity)
        {
        }

        public override void ConfigureRelationships(EntityTypeBuilder<ValidationLink> entity)
        {
            entity.HasOne(r => r.Customer)
                .WithMany(h => h.ValidationLinks)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
