using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tekton.Infrastructure.Interfaces
{
    public interface IWorkUnit :IDisposable
    {
        DbContext DbContext { get; }
        IRepository<T> Repository<T>() where T : class, new();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task Begin();
        Task Commit();
        Task Rollback();
    }
}
