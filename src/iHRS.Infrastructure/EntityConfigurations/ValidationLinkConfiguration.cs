using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class ValidationLinkConfiguration : BaseEntityConfiguration<ValidationLink>
    {
        public override string TableName => "ValidationLinks";
        public override string PrimaryKeyColumnName => "ValidationLinkId";

        public override void ConfigureFields(EntityTypeBuilder<ValidationLink> entity)
        {
        }

        public override void ConfigureRelationships(EntityTypeBuilder<ValidationLink> entity)
        {
        }
    }
}
