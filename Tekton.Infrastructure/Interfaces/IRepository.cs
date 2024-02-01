using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Infrastructure.Interfaces
{
    public interface IRepository<T> where T : class, new()
    {
        public Task<IList<T>> GetAllAsync(params Expression<Func<T, object>>[] includeExpressions);
        public Task<T> GetByIdAsync(object Id, params Expression<Func<T, object>>[] includeExpressions);
        public Task<List<T>> GetByFilterAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeExpressions);
        public Task<T> AddAsync(T entity);
        public Task<T> UpdateAsync(T entity);
        public Task DeleteAsync(object id);

    }
}
