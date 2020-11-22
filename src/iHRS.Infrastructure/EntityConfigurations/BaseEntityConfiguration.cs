using iHRS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        private readonly Guid _tenantId;

        public abstract void ConfigureFields(EntityTypeBuilder<T> entity);
        public abstract void ConfigureRelationships(EntityTypeBuilder<T> entity);

        public abstract IEnumerable<T> SeedData { get; }
        public abstract string TableName { get; }
        public abstract string PrimaryKeyColumnName { get; }

        public BaseEntityConfiguration(Guid tenantId)
        {
            _tenantId = tenantId;
        }

        public void Configure(EntityTypeBuilder<T> entity)
        {
            entity.ToTable(TableName);

            entity.HasQueryFilter(e => e.TenantId == _tenantId && (e.ExpirationDate == null || e.ExpirationDate > DateTime.UtcNow));

            entity.Property(e => e.Id)
                .HasColumnName(PrimaryKeyColumnName)
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.CreatedBy)
                .HasColumnName("CreatedBy")
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("nvarchar(32)")
                .ValueGeneratedNever();

            entity.Property(e => e.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .IsRequired()
                .HasMaxLength(32)
                .HasColumnType("nvarchar(32)")
                .ValueGeneratedNever();

            entity.Property(e => e.CreatedOn)
                .HasColumnName("CreatedOn")
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.ModifiedOn)
                .HasColumnName("ModifiedOn")
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.ExpirationDate)
                .HasColumnName("ExpirationDate")
                .IsRequired(false)
                .ValueGeneratedNever();

            entity.HasOne(e => e.Tenant)
                .WithMany()
                .HasForeignKey(e => e.TenantId);

            if(SeedData?.Any() == true) entity.HasData(SeedData);

            ConfigureFields(entity);
            ConfigureRelationships(entity);
        }

    }
}
