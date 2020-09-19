using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace iHRS.Domain.Common
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> Collection { get; }
        Task<bool> ExistsAsync(Guid id);
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);

        Task<T> GetAsync(Guid id);

        Task<T> GetAsync<TProperty1>(Guid id, Expression<Func<T, TProperty1>> includeExpression1);

        Task<T> GetAsync<TProperty1, TProperty2>(Guid id,
            Expression<Func<T, TProperty1>> includeExpression1,
            Expression<Func<T, TProperty2>> includeExpression2);

        Task<T> GetAsync<TProperty1, TProperty2, TProperty3>(Guid id,
            Expression<Func<T, TProperty1>> includeExpression1,
            Expression<Func<T, TProperty2>> includeExpression2,
            Expression<Func<T, TProperty3>> includeExpression3);
    }
}
