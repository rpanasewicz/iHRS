using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void ConfigureFields(EntityTypeBuilder<User> entity)
        {
            entity.Property(c => c.FirstName)
                .HasColumnType("nvarchar(250")
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(c => c.LastName)
                .HasColumnType("nvarchar(250)")
                .HasMaxLength(250)
                .IsRequired();

            entity.Property(c => c.EmailAddress)
                .HasColumnType("nvarchar(250)")
                .HasMaxLength(250)
                .IsRequired();


            entity.Property(c => c.Password)
                .HasColumnType("nvarchar(250)")
                .HasMaxLength(250)
                .IsRequired();

            entity
                .Property(r => r.DateOfBirth)
                .HasColumnType("datetime2(7)")
                .IsRequired();
        }

        public override void ConfigureRelationships(EntityTypeBuilder<User> entity)
        {
        }

        public override string TableName => "Users";
        public override string PrimaryKeyColumnName => "UserId";
    }
}
