using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class UserConfiguration : EntityBaseConfiguration<User>
    {
        public override void ConfigureFields(EntityTypeBuilder<User> entity)
        {
        }

        public override void ConfigureRelationships(EntityTypeBuilder<User> entity)
        {
        }

        public override string TableName => "Users";
        public override string PrimaryKeyColumnName => "UserId";
    }
}
