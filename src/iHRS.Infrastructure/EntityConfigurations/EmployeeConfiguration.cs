using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace iHRS.Infrastructure.EntityConfigurations
{
    internal class EmployeeConfiguration : BaseEntityConfiguration<Employee>
    {
        public EmployeeConfiguration(HRSContext context) : base(context)
        {
        }

        public override void ConfigureFields(EntityTypeBuilder<Employee> entity)
        {
            entity.Ignore(e => e.Role);

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

        public override void ConfigureRelationships(EntityTypeBuilder<Employee> entity)
        {
        }

        public override string TableName => "Employees";
        public override string PrimaryKeyColumnName => "EmployeeId";

        public override IEnumerable<Employee> SeedData
        {
            get
            {
                var user = Employee.CreateNew("Adam", "Nowak", "user@example.com",
                                          "AQAAAAEAACcQAAAAENU6ixP+jXYINKxOpVeXbTl0X9q83k4cUIXSMPv0iQZro7F2xMN7t7otCg1O3IueJQ==",
                                          new DateTime(1995, 4, 11), Role.TenantOwner);

                user.CreatedBy = "System";
                user.ModifiedBy = "System";
                user.CreatedOn = new DateTime(2020, 1, 1);
                user.ModifiedOn = new DateTime(2020, 1, 1);
                user.TenantId = new Guid("00000000-0000-0000-0000-000000000001");

                typeof(Employee)
                    .GetProperty("Id")
                    .ForceSetValue(user, new Guid("00000000-0000-0000-0000-000000000002"));

                return new Employee[] { user };
            }
        }
    }
}
