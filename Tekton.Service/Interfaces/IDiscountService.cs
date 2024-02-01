using Tekton.Domain.Entities;
using Tekton.Service.DTO;

namespace Tekton.Service.Interfaces
{
    public interface IDiscountService
    {
        public Task<DiscountDTO> GetByIdAsync(long Id);
    }
}