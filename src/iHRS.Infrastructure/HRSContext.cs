using iHRS.Application.Auth;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using iHRS.Infrastructure.EntityConfigurations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
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
        public DbSet<Tenant> Tenants { get; set; }

        private readonly IAuthProvider _authProvider;
        private IDbContextTransaction _currentTransaction;
        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        public HRSContext(DbContextOptions<HRSContext> options, IAuthProvider authProvider) : base(options)
        {
            _authProvider = authProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var tenantId = _authProvider.TenantId;

            modelBuilder.ApplyConfiguration(new TenantTypeConfiguration());

            modelBuilder.ApplyConfiguration(new MessageTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommunicationMethodConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationStatusConfiguration());

            modelBuilder.ApplyConfiguration(new CustomerConfiguration(tenantId));
            modelBuilder.ApplyConfiguration(new HotelConfiguration(tenantId));
            modelBuilder.ApplyConfiguration(new MessageTemplateConfiguration(tenantId));
            modelBuilder.ApplyConfiguration(new ReservationConfiguration(tenantId));
            modelBuilder.ApplyConfiguration(new RoomConfiguration(tenantId));
            modelBuilder.ApplyConfiguration(new UserConfiguration(tenantId));
            modelBuilder.ApplyConfiguration(new ValidationLinkConfiguration(tenantId));            
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAuditables();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditables()
        {
            var auditables = ChangeTracker.Entries<Entity>();
            var userId = _authProvider.UserId != default ? _authProvider.UserId.ToString() : "SYSTEM";
            var tenantId = _authProvider.TenantId;

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
                    entity.Entity.CreatedBy = userId;
                    entity.Entity.CreatedOn = now;
                    entity.Entity.ModifiedBy = userId;
                    entity.Entity.ModifiedOn = now;
                    entity.Entity.TenantId = tenantId;
                }

                if (entity.State == EntityState.Modified)
                {
                    entity.Entity.ModifiedBy = userId;
                    entity.Entity.ModifiedOn = now;
                }
            }
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
}
