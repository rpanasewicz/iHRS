using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    class MessageTemplateConfiguration : EntityBaseConfiguration<MessageTemplate>
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

    class MessageTypeConfiguration : EnumerationBaseConfiguration<MessageType>
    {
        public override string TableName => "MessageTypes";
        public override string PrimaryKeyColumnName => "MessageTypeId";
    }
}
