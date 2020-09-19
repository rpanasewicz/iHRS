using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using iHRS.Application.Common;
using iHRS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace iHRS.Infrastructure.Dispatchers
{
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public CommandDispatcher(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command)
        {
            using var scope = _serviceFactory.CreateScope();

            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
            var handler = scope.ServiceProvider.GetService(handlerType);
            var method = handlerType.GetMethod("Handle");

            return (TResponse)await ((Task<TResponse>)method?.Invoke(handler, new[] { command }));
        }

        public async Task SendAsync(ICommand command)
        {
            using var scope = _serviceFactory.CreateScope();
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = scope.ServiceProvider.GetService(handlerType);

            var method = handlerType.GetMethod("Handle");

            await (Task)method.Invoke(handler, new object[] { command });
        }
    }

    internal sealed class EfCoreRepository<T> : IRepository<T> where T : Entity
    {
        private readonly HRSContext _context;

        public EfCoreRepository(HRSContext context)
        {
            _context = context;
        }

        public IQueryable<T> Collection => _context.Set<T>().AsQueryable();

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        public async Task<T> GetAsync(Guid id)
        {
            var set = _context.Set<T>();
            return await set.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> GetAsync<TProperty>(Guid id, Expression<Func<T, TProperty>> includeExpression)
        {
            var set = _context.Set<T>();

            _context.Set<T>().Include(includeExpression);

            return await set.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> GetAsync<TProperty1, TProperty2>(Guid id,
            Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<T, TProperty2>> includeExpression2)
        {
            var set = _context.Set<T>();

            _context.Set<T>().Include(includeExpression1);
            _context.Set<T>().Include(includeExpression2);

            return await set.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<T> GetAsync<TProperty1, TProperty2, TProperty3>(Guid id,
            Expression<Func<T, TProperty1>> includeExpression1,
            Expression<Func<T, TProperty2>> includeExpression2,
            Expression<Func<T, TProperty3>> includeExpression3)
        {
            var set = _context.Set<T>();

            _context.Set<T>().Include(includeExpression1);
            _context.Set<T>().Include(includeExpression2);
            _context.Set<T>().Include(includeExpression3);

            return await set.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}