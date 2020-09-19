using iHRS.Domain.Common;
using iHRS.Domain.DomainEvents.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iHRS.Domain.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace iHRS.Infrastructure
{
    public class HRSContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public HRSContext(DbContextOptions<HRSContext> options, IHttpContextAccessor httpContextAccessor, IDomainEventPublisher domainEventPublisher) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _domainEventPublisher = domainEventPublisher;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRSContext).Assembly);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await DispatchDomainEvents();
            UpdateAuditables();

            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditables()
        {
            var auditables = ChangeTracker.Entries<Entity>();
            var user = _httpContextAccessor?.HttpContext?.User?.Identity?.Name?.Replace("-", "") ?? "SYSTEM";

            var now = DateTime.UtcNow;

            foreach (var entity in auditables)
            {
                if (entity.State == EntityState.Deleted)
                {
                    entity.State = EntityState.Modified;
                    entity.Entity.ExpirationDate = DateTime.UtcNow;
                }

                if (entity.State == EntityState.Added)
                {
                    entity.Entity.CreatedBy = user;
                    entity.Entity.CreatedOn = now;
                    entity.Entity.ModifiedBy = user;
                    entity.Entity.ModifiedOn = now;
                }

                if (entity.State == EntityState.Modified)
                {
                    entity.Entity.ModifiedBy = user;
                    entity.Entity.ModifiedOn = now;
                }
            }
        }

        private async Task DispatchDomainEvents()
        {
            var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Events)
                .ToList();

            domainEntities
                .ForEach(entity => entity.Entity.ClearEvents());

            foreach (var domainEvent in domainEvents)
                await _domainEventPublisher.PublishAsync(domainEvent);
        }
    }

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
