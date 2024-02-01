using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekton.Domain.Entities;
using Tekton.Service.DTO;

namespace Tekton.UnitTests.Fixtures
{
    public static class DiscountFixture
    {
        public static DiscountDTO GetTestDiscountDTO() => new()
        {
            ProductId = 1,
            Discount = 67,
        };
        public static string GetTestDiscountJSON() => @"{Discount:67,ProductId:1}";
    }
}
