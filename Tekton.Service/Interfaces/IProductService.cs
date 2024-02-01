using Tekton.Domain.Entities;
using Tekton.Service.DTO;

namespace Tekton.Service.Interfaces
{
    public interface IProductService
    {
        public Task<IList<ProductDTO>> GetAllAsync();
        public Task<ProductDTO?> GetByIdAsync(long Id);
        public Task<ProductDTO> AddAsync(NewProductDTO entity);
        public Task<ProductDTO?> UpdateAsync(EditProductDTO entity);
        public Task<ProductDTO> DeleteAsync(long id);
    }
}
