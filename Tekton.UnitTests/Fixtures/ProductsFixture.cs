using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekton.Domain.Entities;
using Tekton.Service.DTO;

namespace Tekton.UnitTests.Fixtures
{
    public static class ProductsFixture
    {
        public static List<ProductDTO> GetTestProductDTOs() => new() {
            new ProductDTO {
                    ProductId = 1,
                    Name = "TV LED 13'",
                    Description = "Samsung LED television",
                    Discount = 0,
                    Price = 100,
                    FinalPrice = 100,
                    StatusName = "Active",
                    Stock = 1
            },
            new ProductDTO {
                    ProductId = 2,
                    Name = "Iphone 12",
                    Description = "Apple Phone",
                    Discount = 50,
                    Price = 2000,
                    FinalPrice = 1000,
                    StatusName = "Active",
                    Stock = 1
            },
            new ProductDTO {
                    ProductId = 3,
                    Name = "Iphone 13",
                    Description = "New Apple Phone",
                    Discount = 25,
                    Price = 2000,
                    FinalPrice = 1500,
                    StatusName = "Active",
                    Stock = 1
            },

        };

        public static NewProductDTO GetTestNewProductDTO() => new()
        {
            Name = new Random().Next(1000).ToString(),
            Description = new Random().Next(1000).ToString(),
            StatusId = 1,
            Price = new Random().Next(1000),
            Stock = 1
        };

        public static EditProductDTO GetTestEditProductDTO() => new()
        {
            Name = new Random().Next(1000).ToString(),
            Description = new Random().Next(1000).ToString(),
            StatusId = 1,
            Price = new Random().Next(1000),
            Stock = 1
        };

        public static Product GetTestInputNewProduct() => new()
        {
            Id = 0,
            Name = "Iphone 13",
            StatusId =1,
            Description = "New Apple Phone",
            Price = 2000,
            Stock = 1
        };

        public static Product GetTestOutputNewProduct() => new()
        {
            Id = 1,
            StatusId = 1,
            Name = "Iphone 13",
            Description = "New Apple Phone",
            Price = 2000,
            Stock = 1
        };

        public static List<Product> GetTestProducts() => new() {
            new Product {
                    Id = 1,
                    Name = "TV LED 13'",
                    Description = "Samsung LED television",
                    Price = 100,
                    StatusId = 1,
                    Stock = 1
            },
            new Product {
                    Id = 2,
                    Name = "Iphone 12",
                    Description = "Apple Phone",
                    Price = 2000,
                    StatusId = 1,
                    Stock = 1
            },
            new Product {
                    Id = 3,
                    Name = "Iphone 13",
                    Description = "New Apple Phone",
                    Price = 2000,
                    StatusId = 1,
                    Stock = 1
            },

        };
    };

}
