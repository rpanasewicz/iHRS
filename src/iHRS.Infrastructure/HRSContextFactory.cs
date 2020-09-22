using System.Threading.Tasks;
using iHRS.Domain.DomainEvents.Abstractions;
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

            return new HRSContext(optionsBuilder.Options, new NullHttpContextAccessor(), new NoEventPublisher());
        }

        private class NullHttpContextAccessor : IHttpContextAccessor
        {
            public HttpContext HttpContext
            {
                get => null;
                set { }
            }
        }

        private class NoEventPublisher : IDomainEventPublisher
        {
            public Task PublishAsync<T>(T @event) where T : class, IDomainEvent
                => Task.CompletedTask;

        }
    }
}