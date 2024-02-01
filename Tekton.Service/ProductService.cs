using System.IO;
using System.Collections;
using Tekton.Infrastructure.Interfaces;
using Tekton.Service.Interfaces;
using Tekton.Domain.Entities;
using Tekton.Service.DTO;
using AutoMapper;
using Tekton.Infrastructure.Repositories.Decorators;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Tekton.Service
{
    public class ProductService : IProductService
    {
        private readonly IWorkUnit _WorkUnit;
        private readonly IMapper _Mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IStatusService _statusService;
        private readonly IDiscountService _discountService;

        public ProductService(IWorkUnit workUnit, IMapper Mapper, IMemoryCache memoryCache, IStatusService statusService, IDiscountService discountService)
        {
            _WorkUnit = workUnit;
            _Mapper = Mapper;
            _memoryCache = memoryCache;
            _statusService = statusService;
            _discountService = discountService;
        }

        public async Task<ProductDTO> GetByIdAsync(long Id)
        {
            var sProduct = await _WorkUnit.Repository<Product>().GetByIdAsync(Id);
            if (sProduct == null) return null;

            var productResult = await LoadProductWithAdditionalInfo(sProduct);
            return productResult;
        }

        public async Task<ProductDTO> LoadProductWithAdditionalInfo(Product product)
        {
            var productResult = _Mapper.Map<ProductDTO>(product);
            var status = await _statusService.GetByIdAsync(product.StatusId);
            if (status != null) productResult.StatusName = status.Name;

            var discount = await _discountService.GetByIdAsync(product.Id);
            if (discount != null)
            {
                productResult.Discount = discount.Discount;
                productResult.FinalPrice = productResult.Price * (100 - discount.Discount) / 100;
            }
            return productResult;
        }

        public async Task<IList<ProductDTO>> GetAllAsync()
        {
            var sProducts = await _WorkUnit.Repository<Product>().GetAllAsync();
            List<ProductDTO> result = new List<ProductDTO>();
            foreach (Product product in sProducts)
            {
                result.Add(await LoadProductWithAdditionalInfo(product));
            }
            return result;
        }


        public async Task<ProductDTO> AddAsync(NewProductDTO entity)
        {
            try
            {
                var sProduct = await _WorkUnit.Repository<Product>().GetByFilterAsync(x => x.Name == entity.Name);
                if (sProduct.Count() > 0) throw new InvalidOperationException($"Ya existe un registro de producto con ese nombre: {entity.Name}");
                if (!await _statusService.ValidateStatusExists(entity.StatusId)) throw new InvalidOperationException($"El valor de StatusId ingresado no es valido: {entity.StatusId}");

                await _WorkUnit.Begin();
                var newProduct = await _WorkUnit.Repository<Product>().AddAsync(_Mapper.Map<Product>(entity));
                await _WorkUnit.Commit();
                return await LoadProductWithAdditionalInfo(newProduct);
            }
            catch (Exception ex)
            {
                await _WorkUnit.Rollback();
                throw;
            }
        }
        public async Task<ProductDTO?> UpdateAsync(EditProductDTO entity)
        {
            try
            {
                if (!await _statusService.ValidateStatusExists(entity.StatusId)) throw new InvalidOperationException($"El valor de StatusId ingresado no es valido: {entity.StatusId}");
                var sProduct = await _WorkUnit.Repository<Product>().GetByIdAsync(entity.ProductId);
                if (sProduct == null) return null;

                sProduct.Name = entity.Name;
                sProduct.StatusId = entity.StatusId;
                sProduct.Stock = entity.Stock;
                sProduct.Description = entity.Description;
                sProduct.Price = entity.Price;

                await _WorkUnit.Begin();
                var updatedProduct = await _WorkUnit.Repository<Product>().UpdateAsync(sProduct);
                await _WorkUnit.Commit();
                return await LoadProductWithAdditionalInfo(updatedProduct);
            }
            catch
            {
                await _WorkUnit.Rollback();
                throw;
            }
        }

        public async Task<ProductDTO> DeleteAsync(long id)
        {
            try
            {
                var sProduct = await _WorkUnit.Repository<Product>().GetByIdAsync(id);
                if (sProduct == null) return null;
                await _WorkUnit.Begin();
                await _WorkUnit.Repository<Product>().DeleteAsync(sProduct.Id);
                await _WorkUnit.Commit();
                return await LoadProductWithAdditionalInfo(sProduct);
            }
            catch
            {
                await _WorkUnit.Rollback();
                throw;
            }
        }
    }
}
