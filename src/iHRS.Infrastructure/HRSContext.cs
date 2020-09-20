using iHRS.Domain.Common;
using iHRS.Domain.DomainEvents.Abstractions;
using iHRS.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace iHRS.Infrastructure
{
    internal class HRSContext : DbContext
    {
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ReservationStatus> ReservationStatuses { get; set; }
        public DbSet<MessageTemplate> MessageTemplate { get; set; }
        public DbSet<MessageType> MessageTemplateType { get; set; }
        public DbSet<CommunicationMethod> CommunicationMethods { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDomainEventPublisher _domainEventPublisher;

        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

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

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
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
