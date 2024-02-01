using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tekton.Infrastructure.Interfaces;

namespace Repositories.Decorators;
public abstract class RepositoryDecorator<TEntity> : IRepository<TEntity> where TEntity : class, new()
{
    protected readonly IRepository<TEntity> _decorated;

    public RepositoryDecorator(IRepository<TEntity> decorated)
    {
        _decorated = decorated;
        
    }

    public abstract Task<TEntity> GetByIdAsync(object Id, params Expression<Func<TEntity, object>>[] includeExpressions);

    public abstract Task<TEntity> AddAsync(TEntity entity);

    public abstract Task DeleteAsync(object id);

    public abstract Task<IList<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeExpressions);

    public abstract Task<List<TEntity>> GetByFilterAsync(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeExpressions);

    public abstract Task<TEntity> UpdateAsync(TEntity entity);



   
}
