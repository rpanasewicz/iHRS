using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace iHRS.Domain.Common
{
    public interface IRepository<T> where T : Entity
    {
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);

        Task LoadProperty<TProperty>(T entity, Expression<Func<T, IEnumerable<TProperty>>> propertyExpression) where TProperty : class;
        Task LoadProperty<TProperty>(T entity, Expression<Func<T, TProperty>> propertyExpression) where TProperty : class;

        Task<T> GetAsync(Guid id);
        Task<T> GetAsync<TProperty1>(Guid id, Expression<Func<T, TProperty1>> includeExpression1);
        Task<T> GetAsync<TProperty1, TProperty2>(Guid id, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<T, TProperty2>> includeExpression2);
        Task<T> GetAsync<TProperty1, TProperty2>(Guid id, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<TProperty1, TProperty2>> thenIncludeExpression);
        Task<T> GetAsync<TProperty1, TProperty2, TProperty3>(Guid id, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<T, TProperty2>> includeExpression2, Expression<Func<T, TProperty3>> includeExpression3);

        Task<T> GetAsync(Expression<Func<T, bool>> filterExpression);
        Task<T> GetAsync<TProperty1>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProperty1>> includeExpression1);
        Task<T> GetAsync<TProperty1, TProperty2>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<T, TProperty2>> includeExpression2);
        Task<T> GetAsync<TProperty1, TProperty2>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<TProperty1, TProperty2>> thenIncludeExpression);
        Task<T> GetAsync<TProperty1, TProperty2, TProperty3>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TProperty1>> includeExpression1, Expression<Func<T, TProperty2>> includeExpression2, Expression<Func<T, TProperty3>> includeExpression3);
        Task<T> FindFromAllAsync(Expression<Func<T, bool>> filterExpression);
    }
}
