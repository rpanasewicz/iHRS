﻿using iHRS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        protected readonly HRSContext _context;

        public abstract void ConfigureFields(EntityTypeBuilder<T> entity);
        public abstract void ConfigureRelationships(EntityTypeBuilder<T> entity);

        public abstract IEnumerable<T> SeedData { get; }
        public abstract string TableName { get; }
        public abstract string PrimaryKeyColumnName { get; }

        public BaseEntityConfiguration(HRSContext context)
        {
            _context = context;
        }

        public void Configure(EntityTypeBuilder<T> entity)
        {
            entity.ToTable(TableName);

            entity.HasQueryFilter(e => e.TenantId == _context.TenantId && (e.ExpirationDate == null || e.ExpirationDate > DateTime.UtcNow));

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

            if (SeedData?.Any() == true)
            {
                var seedlist = SeedData.ToList();

                foreach (var seedEntity in seedlist)
                {
                    seedEntity.CreatedBy = "System";
                    seedEntity.ModifiedBy = "System";
                    seedEntity.CreatedOn = new DateTime(2020, 1, 1);
                    seedEntity.ModifiedOn = new DateTime(2020, 1, 1);
                    seedEntity.TenantId = new Guid("00000000-0000-0000-0000-000000000001");
                }

                entity.HasData(seedlist);
            }

            ConfigureFields(entity);
            ConfigureRelationships(entity);
        }

    }
}
