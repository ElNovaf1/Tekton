using Microsoft.Extensions.Caching.Memory;
using Repositories.Decorators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tekton.Infrastructure.Interfaces;

namespace Tekton.Infrastructure.Repositories.Decorators
{
    public class RepositoryCacheDecorator<TEntity> : RepositoryDecorator<TEntity> where TEntity : class, new()
    {
        private readonly IMemoryCache _memoryCache;
        public RepositoryCacheDecorator(IRepository<TEntity> decorated, IMemoryCache memoryCache)
        : base(decorated) {
            _memoryCache = memoryCache;
        }
        public override async Task<TEntity> GetByIdAsync(object Id, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            string key = $"{typeof(TEntity)}-{Id}";
            return await _memoryCache.GetOrCreateAsync(
                key,
                async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(5));
                    return await _decorated.GetByIdAsync(Id, includeExpressions);
                });
        }

        public override async Task<TEntity> AddAsync(TEntity entity)
        {
            return await ((IRepository<TEntity>)_decorated).AddAsync(entity);
        }

        public override async Task DeleteAsync(object id)
        {
            await((IRepository<TEntity>)_decorated).DeleteAsync(id);
        }

        public override async Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await((IRepository<TEntity>)_decorated).GetAllAsync(includeExpressions);
        }

        public override async Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return await((IRepository<TEntity>)_decorated).GetByFilterAsync(filter, includeExpressions);
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return await((IRepository<TEntity>)_decorated).UpdateAsync(entity);
        }
    }
}
