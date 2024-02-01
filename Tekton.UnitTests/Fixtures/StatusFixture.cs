using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekton.Domain.Entities;
using Tekton.Service.DTO;

namespace Tekton.UnitTests.Fixtures
{
    public static class StatusFixture
    {
        public static Status GetTestStatus() => new()
        {
            Id = 1,
            Name = "Active",
        };
    }
}
