using iHRS.Application.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace iHRS.Infrastructure
{
    internal class HRSContextFactory : IDesignTimeDbContextFactory<HRSContext>
    {
        public HRSContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HRSContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NOT_EXISTING_DATABASE;Integrated Security=True;");

            return new HRSContext(optionsBuilder.Options, new EmptyAuthProvider());
        }

        private class EmptyAuthProvider : IAuthProvider
        {
            public Guid UserId => Guid.Empty;
            public Guid CustomerId => Guid.Empty;
            public Guid TenantId => Guid.Empty;
        }
    }
}