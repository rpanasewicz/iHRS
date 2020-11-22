using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Reflection;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class UserConfiguration : BaseEntityConfiguration<User>
    {
        public UserConfiguration(Guid tenantId) : base(tenantId)
        {
        }

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

            var user = User.CreateNew("Adam", "Nowak", "user@example.com", "AQAAAAEAACcQAAAAENU6ixP+jXYINKxOpVeXbTl0X9q83k4cUIXSMPv0iQZro7F2xMN7t7otCg1O3IueJQ==", new DateTime(1995, 4, 11));

            user.CreatedBy = "System";
            user.CreatedOn = new DateTime(2020, 1, 1);
            user.ModifiedBy = "System";
            user.ModifiedOn = new DateTime(2020, 1, 1);
            user.TenantId = new Guid("00000000-0000-0000-0000-000000000001");

            typeof(User)
                .GetProperty("Id")
                .ForceSetValue(user, new Guid("00000000-0000-0000-0000-000000000002"));

            entity.HasData(user);
        }

        public override void ConfigureRelationships(EntityTypeBuilder<User> entity)
        {
        }

        public override string TableName => "Users";
        public override string PrimaryKeyColumnName => "UserId";
    }
}
