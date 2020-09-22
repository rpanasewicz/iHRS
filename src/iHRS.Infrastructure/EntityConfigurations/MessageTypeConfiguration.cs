using iHRS.Domain.Models;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class MessageTypeConfiguration : BaseEnumerationConfiguration<MessageType>
    {
        public override string TableName => "MessageTypes";
        public override string PrimaryKeyColumnName => "MessageTypeId";
    }
}