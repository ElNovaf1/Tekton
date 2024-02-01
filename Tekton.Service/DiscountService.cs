using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Tekton.Domain.Entities;
using Tekton.Infrastructure;
using Tekton.Infrastructure.Interfaces;
using Tekton.Service.DTO;
using Tekton.Service.Interfaces;

namespace Tekton.Service
{
    public class DiscountService: IDiscountService
{
        private readonly IHttpRepository _httpRepository;
        private readonly IConfiguration _config;

        public DiscountService(IHttpRepository httpRepository, IConfiguration config)
        {
            _httpRepository = httpRepository;
            _config = config;
        }
        public async Task<DiscountDTO> GetByIdAsync(long Id)
        {
            string DiscountApiUri = _config["DiscountAPIUrl"]!;
            var result = new DiscountDTO();
            var discount = await _httpRepository.GetAsync(String.Concat(DiscountApiUri,Id.ToString()));
            if (!string.IsNullOrEmpty(discount)) { 
              return JsonConvert.DeserializeObject<DiscountDTO>(discount)!;
            }
            return null;
        }
    }
}
