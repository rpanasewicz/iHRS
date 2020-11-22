using iHRS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal abstract class BaseEnumerationConfiguration<T> : IEntityTypeConfiguration<T> where T : Enumeration
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

            var enumValues = typeof(Enumeration)
                .GetMethod("GetAll")?
                .MakeGenericMethod(typeof(T))
                .Invoke(null, null) as IEnumerable<T>;

            entity.HasData(enumValues);
        }
    }
}