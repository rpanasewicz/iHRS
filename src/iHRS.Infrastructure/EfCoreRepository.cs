using iHRS.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace iHRS.Infrastructure
{
    internal sealed class EfCoreRepository<T> : IRepository<T> where T : Entity
    {
        private readonly HRSContext _context;

        public EfCoreRepository(HRSContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Set<T>().AnyAsync(e => e.Id == id);
        }

        public Task<T> GetAsync(Guid id)
            => GetAsync(e => e.Id == id);

        public Task<T> GetAsync<TProperty1>(Guid id, Expression<Func<T, TProperty1>> includeExpression1)
            => GetAsync(e => e.Id == id, includeExpression1);

        public Task<T> GetAsync<TProperty1, TProperty2>(Guid id, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<T, TProperty2>> includeExpression2)
            => GetAsync(e => e.Id == id, includeExpression1, includeExpression2);

        public Task<T> GetAsync<TProperty1, TProperty2>(Guid id, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<TProperty1, TProperty2>> thenIncludeExpression)
            => GetAsync(e => e.Id == id, includeExpression1, thenIncludeExpression);


        public Task<T> GetAsync<TProperty1, TProperty2, TProperty3>(Guid id, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<T, TProperty2>> includeExpression2, Expression<Func<T, TProperty3>> includeExpression3)
            => GetAsync(e => e.Id == id, includeExpression1, includeExpression2, includeExpression3);

        public async Task<T> GetAsync(Expression<Func<T, bool>> filterExpression)
        {
            return await _context.Set<T>()
                .SingleOrDefaultAsync(filterExpression);
        }

        public async Task<T> GetAsync<TProperty1>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProperty1>> includeExpression1)
        {
            return await _context.Set<T>()
                .Include(includeExpression1)
                .SingleOrDefaultAsync(filterExpression);
        }

        public async Task<T> GetAsync<TProperty1, TProperty2>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<T, TProperty2>> includeExpression2)
        {
            return await _context.Set<T>()
                .Include(includeExpression1)
                .Include(includeExpression2)
                .SingleOrDefaultAsync(filterExpression);
        }

        public async Task<T> GetAsync<TProperty1, TProperty2>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<TProperty1, TProperty2>> thenIncludeExpression)
        {
            return await _context.Set<T>()
                .Include(includeExpression1)
                .ThenInclude(thenIncludeExpression)
                .SingleOrDefaultAsync(filterExpression);
        }

        public async Task<T> GetAsync<TProperty1, TProperty2, TProperty3>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<T, TProperty2>> includeExpression2, Expression<Func<T, TProperty3>> includeExpression3)
        {
            return await _context.Set<T>()
                .Include(includeExpression1)
                .Include(includeExpression2)
                .Include(includeExpression3)
                .SingleOrDefaultAsync(filterExpression);
        }

        public async Task LoadProperty<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class
            => await _context.Entry(entity).Collection(propertyExpression).LoadAsync();

        public async Task LoadProperty<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class
            => await _context.Entry(entity).Reference(propertyExpression).LoadAsync();
    }
}