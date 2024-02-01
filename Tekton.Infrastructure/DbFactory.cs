using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Infrastructure
{
    public class DbFactory : IDisposable
    {
        private bool _isDisposed;
        private Func<TektonContext> _instanceFunc;
        private DbContext _dbContext;
        public DbContext DbContext => _dbContext ?? (_dbContext = _instanceFunc.Invoke());

        public DbFactory(Func<TektonContext> dbContextFactory)
        {
            _instanceFunc = dbContextFactory;
        }

        public void Dispose()
        {
            if (!_isDisposed && _dbContext != null)
            {
                _isDisposed = true;
                _dbContext.Dispose();
            }
        }
    }
}
