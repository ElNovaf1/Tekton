using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tekton.Service.DTO
{
    [DisplayName("EditProduct")]
    public class EditProductDTO
{
        [Required(ErrorMessage = "Id del producto es requerido")]
        [JsonProperty("ProductId", Required = Required.Always)]
        public long ProductId { get; set; }

        [Required(ErrorMessage = "Nombre de producto es requerido")]
        [StringLength(100)]
        [MinLength(3)]
        [JsonProperty("Name", Required = Required.Always)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Status del producto es requerido")]
        [JsonProperty("StatusId", Required = Required.Always)]
        public byte StatusId { get; set; }

        [Required(ErrorMessage = "Stock actual del producto es requerido")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El valor de Stock no puede ser menor que cero")]
        [JsonProperty("Stock", Required = Required.Always)]
        public decimal Stock { get; set; }

        [Required(ErrorMessage = "Descripción del producto es requerida")]
        [StringLength(500)]
        [MinLength(3)]
        [JsonProperty("Description", Required = Required.Always)]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Precio del producto es requerido")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "El precio no puede ser menor que cero")]
        [JsonProperty("Price", Required = Required.Always)]
        public decimal Price { get; set; }

    }
}
