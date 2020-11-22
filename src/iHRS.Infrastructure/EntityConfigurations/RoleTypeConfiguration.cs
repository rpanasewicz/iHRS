using iHRS.Domain.Models;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class RoleTypeConfiguration : BaseEnumerationConfiguration<Role>
    {
        public override string TableName => "Roles";
        public override string PrimaryKeyColumnName => "RoleId";
    }
}
