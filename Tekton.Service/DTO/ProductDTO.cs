using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Tekton.Service.DTO
{
    [DisplayName("Product")]
    public class ProductDTO
    {
        public long ProductId { get; set; }
        public string Name { get; set; } = "";
        public string StatusName { get; set; } = "";
        public decimal Stock { get; set; }
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
