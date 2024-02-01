using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Tekton.API.Controllers;
using Tekton.Domain.Entities;
using Tekton.Infrastructure.Interfaces;
using Tekton.Infrastructure.Repositories;
using Tekton.Service;
using Tekton.Service.DTO;
using Tekton.Service.Interfaces;
using Tekton.Service.Profiles;
using Tekton.UnitTests.Fixtures;


namespace Tekton.UnitTests.Systems.Services
{
    public class TestDiscountService
    {
        IConfiguration _config;
        public TestDiscountService()
        {
            _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        [Fact]
        public async Task GetByID_WhenCalled_ReturnsDiscount()
        {
            byte testId = 1;
            string DiscountApiUri = _config["DiscountAPIUrl"]!;
            DiscountApiUri = String.Concat(DiscountApiUri, testId.ToString());
            string mockResult = DiscountFixture.GetTestDiscountJSON();

            //SETUP
            var RepositoryMock = new Mock<IHttpRepository>();
            RepositoryMock.Setup(m => m.GetAsync(DiscountApiUri)).ReturnsAsync(mockResult);
            var ServiceMock = new DiscountService(RepositoryMock.Object, _config);

            //ACTIONS
            var testGetById = await ServiceMock.GetByIdAsync(testId);
            //ASSERTIONS
            testGetById.Should().NotBeNull();
            testGetById.Should().BeOfType<DiscountDTO>();
            RepositoryMock.Verify(
               service => service.GetAsync(DiscountApiUri),
               Times.Once()
           );
        }
    }
}
