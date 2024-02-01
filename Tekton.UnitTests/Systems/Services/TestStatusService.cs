using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Tekton.API.Controllers;
using Tekton.Domain.Entities;
using Tekton.Infrastructure.Interfaces;
using Tekton.Service;
using Tekton.Service.DTO;
using Tekton.Service.Interfaces;
using Tekton.Service.Profiles;
using Tekton.UnitTests.Fixtures;


namespace Tekton.UnitTests.Systems.Services
{
    public class TestStatusService
    {
        IMapper _mapper;
        MapperConfiguration _config;
        IMemoryCache _memoryCache;
        public TestStatusService()
        {
            _config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = _config.CreateMapper();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async Task GetByID_WhenCalled_ReturnsStatus()
        {
            byte testId = 1;
            
            //SETUP
            var RepositoryMock = new Mock<IRepository<Status>>();
            RepositoryMock.Setup(m => m.GetByIdAsync(testId)).ReturnsAsync(StatusFixture.GetTestStatus());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Status>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            var ServiceMock = new StatusService(unitOfWorkMock.Object, _memoryCache);

            //ACTIONS
            var testGetById = await ServiceMock.GetByIdAsync(testId);
            //ASSERTIONS
            testGetById.Should().NotBeNull();
            testGetById.Should().BeOfType<Status>();
            Assert.True(testGetById.Id > 0);
        }
        [Fact]
        public async Task GetByID_WhenCalled_UsesCache()
        {
            byte testId = 1;
            string cachedStatusKey = "Tekton.Domain.Entities.Status-1";

            //SETUP
            var RepositoryMock = new Mock<IRepository<Status>>();
            RepositoryMock.Setup(m => m.GetByIdAsync(testId)).ReturnsAsync(StatusFixture.GetTestStatus());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Status>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            var ServiceMock = new StatusService(unitOfWorkMock.Object, _memoryCache);
            _memoryCache.Remove(cachedStatusKey);

            //ACTIONS
            var testGetById = await ServiceMock.GetByIdAsync(testId);
            testGetById = await ServiceMock.GetByIdAsync(testId);
            var cachedStatus = _memoryCache.Get(cachedStatusKey);
            //ASSERTIONS
            RepositoryMock.Verify(
                service => service.GetByIdAsync(testId),
                Times.Once()
            );
        }

        [Fact]
        public async Task ValidateStatusID_WhenCalled_ReturnsTrue()
        {
            byte testId = 1;

            //SETUP
            var RepositoryMock = new Mock<IRepository<Status>>();
            RepositoryMock.Setup(m => m.GetByIdAsync(testId)).ReturnsAsync(StatusFixture.GetTestStatus());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Status>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            var ServiceMock = new StatusService(unitOfWorkMock.Object, _memoryCache);

            //ACTIONS
            var testGetById = await ServiceMock.ValidateStatusExists(testId);
            //ASSERTIONS
            testGetById.Should().BeTrue();
        }

    }
}
