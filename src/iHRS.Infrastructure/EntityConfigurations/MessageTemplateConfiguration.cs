using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class MessageTemplateConfiguration : BaseEntityConfiguration<MessageTemplate>
    {
        public MessageTemplateConfiguration(Guid tenantId) : base(tenantId)
        {
        }

        public override void ConfigureFields(EntityTypeBuilder<MessageTemplate> entity)
        {
            entity.Ignore(e => e.MessageType);
            entity.Ignore(e => e.CommunicationMethod);

            entity.Property(e => e.Message)
                .HasColumnType("nvarchar(max)")
                .IsRequired();
        }

        public override void ConfigureRelationships(EntityTypeBuilder<MessageTemplate> entity)
        {
            entity.HasOne(e => e.Hotel)
                .WithMany(h => h.MessageTemplates)
                .HasForeignKey(e => e.HotelId)
                .OnDelete(DeleteBehavior.NoAction);
        }

        public override string TableName => "MessageTemplates";
        public override string PrimaryKeyColumnName => "MessageTemplateId";
    }
}
