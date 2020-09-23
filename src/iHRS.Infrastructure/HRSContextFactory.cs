using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace iHRS.Infrastructure
{
    internal class HRSContextFactory : IDesignTimeDbContextFactory<HRSContext>
    {
        public HRSContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HRSContext>();
            optionsBuilder.UseSqlServer(
                "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NOT_EXISTING_DATABASE;Integrated Security=True;");

            return new HRSContext(optionsBuilder.Options, new NullHttpContextAccessor());
        }

        private class NullHttpContextAccessor : IHttpContextAccessor
        {
            public HttpContext HttpContext
            {
                get => null;
                set { }
            }
        }
    }
}