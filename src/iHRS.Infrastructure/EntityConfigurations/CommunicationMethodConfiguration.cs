using iHRS.Domain.Models;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class CommunicationMethodConfiguration : BaseEnumerationConfiguration<CommunicationMethod>
    {
        public override string TableName => "CommunicationMethods";
        public override string PrimaryKeyColumnName => "CommunicationMethodId";
    }
}
