using Tekton.Infrastructure.Interfaces;
using Tekton.Service.Interfaces;
using Tekton.Domain.Entities;
using Tekton.Service.DTO;
using AutoMapper;
using Repositories.Decorators;
using Tekton.Infrastructure.Repositories.Decorators;
using Microsoft.Extensions.Caching.Memory;

namespace Tekton.Service
{
    public class StatusService : IStatusService
{
        private readonly IWorkUnit _WorkUnit;
        private readonly IMemoryCache _memoryCache;
        public StatusService(IWorkUnit workUnit, IMemoryCache memoryCache)
        {
            _WorkUnit = workUnit;
            _memoryCache = memoryCache;
        }

        public async Task<Status> GetByIdAsync(byte Id)
        {
            var repo = _WorkUnit.Repository<Status>();
            //APLICANDO DECORATOR PARA ALMACENAR/OBTENER RESULTADO EN CACHE
            var decorator = new RepositoryCacheDecorator<Status>(repo, _memoryCache);
            return await decorator.GetByIdAsync(Id);
        }

        public async Task<bool> ValidateStatusExists(byte StatusId)
        {
            var status = await GetByIdAsync(StatusId);
            return status == null ? false : true;
        }
    }
}
