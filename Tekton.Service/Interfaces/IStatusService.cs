using Tekton.Domain.Entities;
using Tekton.Service.DTO;

namespace Tekton.Service.Interfaces
{
    public interface IStatusService
{
        public Task<Status> GetByIdAsync(byte Id);
        public Task<bool> ValidateStatusExists(byte StatusId);
    }
}
