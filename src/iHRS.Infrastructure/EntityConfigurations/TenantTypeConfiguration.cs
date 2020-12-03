using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace iHRS.Infrastructure.EntityConfigurations
{
    public class TenantTypeConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(t => t.Id);

            builder.HasData(new Tenant(new Guid("00000000-0000-0000-0000-000000000001"), "My first hotel chain"));
        }
    }
}
