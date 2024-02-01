using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tekton.Domain.Entities;

public partial class Product: EntityBase<long>
{
    [Required(ErrorMessage = "Nombre de producto es requerido")]
    [StringLength(100)]
    [MinLength(3)]
    public string Name { get; set; }

    [Required(ErrorMessage = "Status del producto es requerido")]
    public byte StatusId { get; set; }

    [Required(ErrorMessage = "Stock actual del producto es requerido")]
    public decimal Stock { get; set; }

    [Required(ErrorMessage = "Descripción del producto es requerida")]
    [StringLength(500)]
    [MinLength(3)]
    public string Description { get; set; }

    [Required(ErrorMessage = "Precio del producto es requerido")]
    public decimal Price { get; set; }
}
