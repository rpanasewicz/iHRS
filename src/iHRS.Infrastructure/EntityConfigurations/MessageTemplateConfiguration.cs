using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class MessageTemplateConfiguration : BaseEntityConfiguration<MessageTemplate>
    {
        public override void ConfigureFields(EntityTypeBuilder<MessageTemplate> entity)
        {

        }

        public override void ConfigureRelationships(EntityTypeBuilder<MessageTemplate> entity)
        {
        }

        public override string TableName => "MessageTemplates";
        public override string PrimaryKeyColumnName => "MessageTemplateId";
    }
}
