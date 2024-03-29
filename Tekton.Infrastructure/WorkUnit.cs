﻿using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tekton.Infrastructure.Interfaces;
using Tekton.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Tekton.Infrastructure
{
    public class WorkUnit : IWorkUnit
    {
        public DbContext DbContext { get; private set; }
        private Dictionary<string, object> Repositories { get; }
        private IDbContextTransaction _transaction;
        private IsolationLevel? _isolationLevel;
      
        public WorkUnit(DbFactory dbFactory)
        {
            DbContext = dbFactory.DbContext;
            Repositories = new Dictionary<string, dynamic>();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task StartNewTransactionIfNeeded()
        {
            if (_transaction == null)
            {
                _transaction = _isolationLevel.HasValue ?
                    await DbContext.Database.BeginTransactionAsync(_isolationLevel.GetValueOrDefault()) : await DbContext.Database.BeginTransactionAsync();
            }
        }

        public async Task Begin()
        {
            await StartNewTransactionIfNeeded();
        }

        public async Task Commit()
        {
            await DbContext.SaveChangesAsync();

            if (_transaction == null) return;
            await _transaction.CommitAsync();

            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task Rollback()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();

            await _transaction.DisposeAsync();
            _transaction = null;
        }


        public void Dispose()
        {
            try {
                if (DbContext == null)
                    return;
                //
                // Close connection
                if (DbContext.Database.GetDbConnection().State == ConnectionState.Open)
                {
                    DbContext.Database.GetDbConnection().Close();
                }
                DbContext.Dispose();

                DbContext = null;
            }
            catch (ObjectDisposedException)  {
            }
        }

        public IRepository<T> Repository<T>() where T : class, new()
        {
            var type = typeof(T);
            var typeName = type.Name;

            lock (Repositories)
            {
                if (Repositories.ContainsKey(typeName))
                {
                    return (IRepository<T>)Repositories[typeName];
                }

                var repository = new Repository<T>(DbContext);

                Repositories.Add(typeName, repository);
                return repository;
            }
        }
    }
}
