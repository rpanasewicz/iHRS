using iHRS.Domain.Models;

namespace iHRS.Infrastructure.EntityConfigurations
{
    class CommunicationMethodConfiguration : EnumerationBaseConfiguration<CommunicationMethod>
    {
        public override string TableName => "CommunicationMethods";
        public override string PrimaryKeyColumnName => "CommunicationMethodId";
    }
}
