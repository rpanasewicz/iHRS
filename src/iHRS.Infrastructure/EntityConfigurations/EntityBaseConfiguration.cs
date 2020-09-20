﻿using iHRS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;
using System.Threading.Channels;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal abstract class EntityBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity
    {
        public abstract void ConfigureFields(EntityTypeBuilder<T> entity);
        public abstract void ConfigureRelationships(EntityTypeBuilder<T> entity);
        public abstract string TableName { get; }
        public abstract string PrimaryKeyColumnName { get; }

        public void Configure(EntityTypeBuilder<T> entity)
        {
            entity.ToTable(TableName);

            entity.HasQueryFilter(e => e.ExpirationDate == null || e.ExpirationDate > DateTime.UtcNow);

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

            typeof(T)
                .GetProperties()
                .Where(p => p.PropertyType.IsSubclassOf(typeof(Enumeration)) && p.PropertyType.IsClass && !p.PropertyType.IsAbstract)
                .Select(p => p.Name)
                .ToList()
                .ForEach(p => entity.Ignore(p));

            ConfigureFields(entity);
            ConfigureRelationships(entity);
        }

    }

    internal abstract class EnumerationBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : Enumeration
    {
        public abstract string TableName { get; }
        public abstract string PrimaryKeyColumnName { get; }

        public void Configure(EntityTypeBuilder<T> entity)
        {
            entity.ToTable(TableName);

            entity.Property(e => e.Id)
                .HasColumnName(PrimaryKeyColumnName)
                .IsRequired()
                .ValueGeneratedNever();

            entity.Property(e => e.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(128)")
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}
