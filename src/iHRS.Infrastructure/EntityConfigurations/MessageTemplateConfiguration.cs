using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class MessageTemplateConfiguration : BaseEntityConfiguration<MessageTemplate>
    {
        public MessageTemplateConfiguration(HRSContext context) : base(context)
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
        public override IEnumerable<MessageTemplate> SeedData => null;
    }
}
