using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekton.Domain.Entities;
using Tekton.Infrastructure;
using Tekton.Infrastructure.Interfaces;
using Tekton.Service.Interfaces;
using Tekton.Service;
using Tekton.UnitTests.Fixtures;
using FluentAssertions;
using Castle.DynamicProxy;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using Tekton.Infrastructure.Repositories;
using System.Linq.Expressions;
using Tekton.Service.DTO;

namespace Tekton.UnitTests.Systems.Infrastructure
{
    public class TestHttpRepository
    {
        IConfiguration _config;
        public TestHttpRepository()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        [Fact]
        public async Task GetAsync_WhenCalledAndSuccess_ReturnsStringContent()
        {
            byte testId = 1;
            string DiscountApiUri = _config["DiscountAPIUrl"]!;
            DiscountApiUri = String.Concat(DiscountApiUri, testId.ToString());
            string mockResult = DiscountFixture.GetTestDiscountJSON();

            //SETUP
            var ServiceMock = new HttpRepository();

            //ACTIONS
            var testGet = await ServiceMock.GetAsync(DiscountApiUri);
            //ASSERTIONS
            testGet.Should().NotBeNull();
            testGet.Should().BeOfType<string>();
            testGet.Should().NotBeEmpty();
        }
    }
}
