using iHRS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
        }
    }
}