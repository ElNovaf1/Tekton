using AutoMapper;
using FluentAssertions;
using Moq;
using Tekton.Infrastructure.Interfaces;
using Tekton.Service;
using Tekton.Service.DTO;
using Tekton.Service.Interfaces;
using Tekton.UnitTests.Fixtures;
using Microsoft.Extensions.Caching.Memory;
using Tekton.Service.Profiles;
using Tekton.Domain.Entities;
using System.Linq.Expressions;

namespace Tekton.UnitTests.Systems.Services
{
    public class TestProductService
    {
        MapperConfiguration _config;
        IMapper _mapper;
        IMemoryCache _memoryCache;

        public TestProductService()
        {
            _config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfiles>());
            _mapper = _config.CreateMapper();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        [Fact]
        public async Task GetById_WhenCalled_ReturnsProductDTO()
        {
            long testId = 1;
            byte testStatusId = 1;

            //SETUP
            var RepositoryMock = new Mock<IRepository<Product>>();
            RepositoryMock.Setup(m => m.GetByIdAsync(testId)).ReturnsAsync(ProductsFixture.GetTestProducts()[0]);

            var RepositoryStatusMock = new Mock<IStatusService>();
            RepositoryStatusMock.Setup(m => m.GetByIdAsync(testStatusId)).ReturnsAsync(StatusFixture.GetTestStatus());

            var RepositoryDiscountMock = new Mock<IDiscountService>();
            RepositoryDiscountMock.Setup(m => m.GetByIdAsync(testId)).ReturnsAsync(DiscountFixture.GetTestDiscountDTO());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            var ServiceMock = new ProductService(unitOfWorkMock.Object, _mapper, _memoryCache, RepositoryStatusMock.Object, RepositoryDiscountMock.Object);

            //ACTIONS
            var testGetById = await ServiceMock.GetByIdAsync(testId);
            //ASSERTIONS
            testGetById.Should().NotBeNull();
            testGetById.Should().BeOfType<ProductDTO>();
            Assert.True(testGetById.ProductId > 0);
        }
        [Fact]
        public async Task LoadProductAdditionalInfo_WhenCalled_ReturnsProductDTO_With_Additional_info()
        {
            long testId = 1;
            byte testStatusId = 1;

            //SETUP
            var RepositoryMock = new Mock<IRepository<Product>>();
            RepositoryMock.Setup(m => m.GetByIdAsync(testId)).ReturnsAsync(ProductsFixture.GetTestProducts()[0]);

            var RepositoryStatusMock = new Mock<IStatusService>();
            RepositoryStatusMock.Setup(m => m.GetByIdAsync(testStatusId)).ReturnsAsync(StatusFixture.GetTestStatus());

            var RepositoryDiscountMock = new Mock<IDiscountService>();
            RepositoryDiscountMock.Setup(m => m.GetByIdAsync(testId)).ReturnsAsync(DiscountFixture.GetTestDiscountDTO());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            var ServiceMock = new ProductService(unitOfWorkMock.Object, _mapper, _memoryCache, RepositoryStatusMock.Object, RepositoryDiscountMock.Object);

            //ACTIONS
            var testLoadInfo = await ServiceMock.LoadProductWithAdditionalInfo(ProductsFixture.GetTestProducts()[0]);
            //ASSERTIONS
            testLoadInfo.Should().NotBeNull();
            testLoadInfo.Should().BeOfType<ProductDTO>();
            Assert.True(testLoadInfo.ProductId > 0);
            Assert.True(testLoadInfo.FinalPrice > 0);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsListOfProductDTO()
        {
            long testId = 1;
            byte testStatusId = 1;

            //SETUP
            var RepositoryMock = new Mock<IRepository<Product>>();
            RepositoryMock.Setup(m => m.GetAllAsync()).ReturnsAsync(ProductsFixture.GetTestProducts());

            var RepositoryStatusMock = new Mock<IStatusService>();
            RepositoryStatusMock.Setup(m => m.GetByIdAsync(testStatusId)).ReturnsAsync(StatusFixture.GetTestStatus());

            var RepositoryDiscountMock = new Mock<IDiscountService>();
            RepositoryDiscountMock.Setup(m => m.GetByIdAsync(testId)).ReturnsAsync(DiscountFixture.GetTestDiscountDTO());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            var ServiceMock = new ProductService(unitOfWorkMock.Object, _mapper, _memoryCache, RepositoryStatusMock.Object, RepositoryDiscountMock.Object);

            //ACTIONS
            var testGetById = await ServiceMock.GetAllAsync();
            //ASSERTIONS
            testGetById.Should().NotBeNull();
            testGetById.Should().BeOfType<List<ProductDTO>>();
            testGetById.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task AddAsync_WhenCalledAndProductNotExists_ReturnsInsertedProductDTO()
        {
            Product mockInputProduct = ProductsFixture.GetTestProducts()[0];
            Product mockOutputProduct = ProductsFixture.GetTestProducts()[0];

            var MockInputProductDTO = _mapper.Map<NewProductDTO>(mockInputProduct);

            //SETUPS
            var RepositoryMock = new Mock<IRepository<Product>>();
            RepositoryMock.Setup(m => m.AddAsync(It.IsAny<Product>())).ReturnsAsync(mockOutputProduct);
            RepositoryMock.Setup(m => m.GetByFilterAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(new List<Product>());

            var RepositoryStatusMock = new Mock<IStatusService>();
            RepositoryStatusMock.Setup(m => m.GetByIdAsync(It.IsAny<byte>())).ReturnsAsync(StatusFixture.GetTestStatus());
            RepositoryStatusMock.Setup(m => m.ValidateStatusExists(mockInputProduct.StatusId)).ReturnsAsync(true);

            var RepositoryDiscountMock = new Mock<IDiscountService>();
            RepositoryDiscountMock.Setup(m => m.GetByIdAsync(mockInputProduct.Id)).ReturnsAsync(DiscountFixture.GetTestDiscountDTO());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            IProductService ServiceMock = new ProductService(unitOfWorkMock.Object, _mapper,_memoryCache, RepositoryStatusMock.Object, RepositoryDiscountMock.Object);

            //ACTIONS
            var testAdd = await ServiceMock.AddAsync(MockInputProductDTO);
            //ASSERTIONS
            testAdd.Should().NotBeNull();
            testAdd.Should().BeOfType<ProductDTO>();
        }

        [Fact]
        public async Task AddAsync_WhenCalledAndProductExists_ReturnsException()
        {
            Product mockInputProduct = ProductsFixture.GetTestProducts()[0];
            Product mockOutputProduct = ProductsFixture.GetTestProducts()[0];

            var MockInputProductDTO = _mapper.Map<NewProductDTO>(mockInputProduct);

            //CREANDO MOCKS DE REPOSITORIOS Y UNIDAD DE TRABAJO
            var RepositoryMock = new Mock<IRepository<Product>>();
            RepositoryMock.Setup(m => m.AddAsync(It.IsAny<Product>())).ReturnsAsync(mockOutputProduct);
            RepositoryMock.Setup(m => m.GetByFilterAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(new List<Product>() { mockOutputProduct });

            var RepositoryStatusMock = new Mock<IStatusService>();
            RepositoryStatusMock.Setup(m => m.GetByIdAsync(It.IsAny<byte>())).ReturnsAsync(StatusFixture.GetTestStatus());
            RepositoryStatusMock.Setup(m => m.ValidateStatusExists(mockInputProduct.StatusId)).ReturnsAsync(true);

            var RepositoryDiscountMock = new Mock<IDiscountService>();
            RepositoryDiscountMock.Setup(m => m.GetByIdAsync(mockInputProduct.Id)).ReturnsAsync(DiscountFixture.GetTestDiscountDTO());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            IProductService ServiceMock = new ProductService(unitOfWorkMock.Object, _mapper, _memoryCache, RepositoryStatusMock.Object, RepositoryDiscountMock.Object);

            //TESTEANDO
            Func<Task> act = () => ServiceMock.AddAsync(MockInputProductDTO);
            //ASSERT
            InvalidOperationException exception = await Assert.ThrowsAsync<InvalidOperationException>(act);
            Assert.Equal($"Ya existe un registro de producto con ese nombre: {MockInputProductDTO.Name}", exception.Message);
        }
       
        [Fact]
        public async Task UpdateAsync_WhenCalledAndProductExists_ReturnsUpdatedProductDTO()
        {
            EditProductDTO mockProductDTO = ProductsFixture.GetTestEditProductDTO();
            Product mockProduct = _mapper.Map<Product>(mockProductDTO);

            //var MockInputProductDTO = 

            //SETUPS
            var RepositoryMock = new Mock<IRepository<Product>>();
            RepositoryMock.Setup(m => m.UpdateAsync(mockProduct)).ReturnsAsync(mockProduct);
            RepositoryMock.Setup(m => m.GetByFilterAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(new List<Product>());
            RepositoryMock.Setup(m => m.GetByIdAsync(mockProduct.Id)).ReturnsAsync(mockProduct);

            var RepositoryStatusMock = new Mock<IStatusService>();
            RepositoryStatusMock.Setup(m => m.GetByIdAsync(It.IsAny<byte>())).ReturnsAsync(StatusFixture.GetTestStatus());
            RepositoryStatusMock.Setup(m => m.ValidateStatusExists(mockProduct.StatusId)).ReturnsAsync(true);

            var RepositoryDiscountMock = new Mock<IDiscountService>();
            RepositoryDiscountMock.Setup(m => m.GetByIdAsync(mockProduct.Id)).ReturnsAsync(DiscountFixture.GetTestDiscountDTO());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            IProductService ServiceMock = new ProductService(unitOfWorkMock.Object, _mapper, _memoryCache, RepositoryStatusMock.Object, RepositoryDiscountMock.Object);

            //ACTIONS
            var testAdd = await ServiceMock.UpdateAsync(mockProductDTO);
            //ASSERTIONS
            testAdd.Should().NotBeNull();
            testAdd.Should().BeOfType<ProductDTO>();
        }

        [Fact]
        public async Task UpdateAsync_WhenCalledAndProductNotExists_ReturnsNull()
        {
            EditProductDTO mockProductDTO = ProductsFixture.GetTestEditProductDTO();
            Product mockProduct = _mapper.Map<Product>(mockProductDTO);

            //var MockInputProductDTO = 

            //SETUPS
            var RepositoryMock = new Mock<IRepository<Product>>();
            RepositoryMock.Setup(m => m.UpdateAsync(mockProduct)).ReturnsAsync(mockProduct);
            RepositoryMock.Setup(m => m.GetByFilterAsync(It.IsAny<Expression<Func<Product, bool>>>())).ReturnsAsync(new List<Product>());
            RepositoryMock.Setup(m => m.GetByIdAsync(mockProduct.Id)).ReturnsAsync(() => null);

            var RepositoryStatusMock = new Mock<IStatusService>();
            RepositoryStatusMock.Setup(m => m.GetByIdAsync(It.IsAny<byte>())).ReturnsAsync(StatusFixture.GetTestStatus());
            RepositoryStatusMock.Setup(m => m.ValidateStatusExists(mockProduct.StatusId)).ReturnsAsync(true);

            var RepositoryDiscountMock = new Mock<IDiscountService>();
            RepositoryDiscountMock.Setup(m => m.GetByIdAsync(mockProduct.Id)).ReturnsAsync(DiscountFixture.GetTestDiscountDTO());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            IProductService ServiceMock = new ProductService(unitOfWorkMock.Object, _mapper, _memoryCache, RepositoryStatusMock.Object, RepositoryDiscountMock.Object);

            //ACTIONS
            var testAdd = await ServiceMock.UpdateAsync(mockProductDTO);
            //ASSERTIONS
            testAdd.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_WhenCalledAndProductExists_ReturnsDeletedProductDTO()
        {
            long testId = 1;
            Product mockProduct = ProductsFixture.GetTestProducts()[0];

            //SETUPS
            var RepositoryMock = new Mock<IRepository<Product>>();
            RepositoryMock.Setup(m => m.DeleteAsync(testId)).Returns(Task.CompletedTask);
            RepositoryMock.Setup(m => m.GetByIdAsync(mockProduct.Id)).ReturnsAsync(mockProduct);

            var RepositoryStatusMock = new Mock<IStatusService>();
            RepositoryStatusMock.Setup(m => m.GetByIdAsync(It.IsAny<byte>())).ReturnsAsync(StatusFixture.GetTestStatus());
            RepositoryStatusMock.Setup(m => m.ValidateStatusExists(mockProduct.StatusId)).ReturnsAsync(true);

            var RepositoryDiscountMock = new Mock<IDiscountService>();
            RepositoryDiscountMock.Setup(m => m.GetByIdAsync(mockProduct.Id)).ReturnsAsync(DiscountFixture.GetTestDiscountDTO());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            IProductService ServiceMock = new ProductService(unitOfWorkMock.Object, _mapper, _memoryCache, RepositoryStatusMock.Object, RepositoryDiscountMock.Object);

            //ACTIONS
            var testAdd = await ServiceMock.DeleteAsync(testId);
            //ASSERTIONS
            testAdd.Should().NotBeNull();
            testAdd.Should().BeOfType<ProductDTO>();
        }

        [Fact]
        public async Task DeleteAsync_WhenCalledAndProductNotExists_ReturnsNull()
        {
            long testId = 1;
            Product mockProduct = ProductsFixture.GetTestProducts()[0];

            //SETUPS
            var RepositoryMock = new Mock<IRepository<Product>>();
            RepositoryMock.Setup(m => m.DeleteAsync(testId)).Returns(Task.CompletedTask);
            RepositoryMock.Setup(m => m.GetByIdAsync(mockProduct.Id)).ReturnsAsync(() => null);

            var RepositoryStatusMock = new Mock<IStatusService>();
            RepositoryStatusMock.Setup(m => m.GetByIdAsync(It.IsAny<byte>())).ReturnsAsync(StatusFixture.GetTestStatus());
            RepositoryStatusMock.Setup(m => m.ValidateStatusExists(mockProduct.StatusId)).ReturnsAsync(true);

            var RepositoryDiscountMock = new Mock<IDiscountService>();
            RepositoryDiscountMock.Setup(m => m.GetByIdAsync(mockProduct.Id)).ReturnsAsync(DiscountFixture.GetTestDiscountDTO());

            var unitOfWorkMock = new Mock<IWorkUnit>();
            unitOfWorkMock.Setup(m => m.Repository<Product>()).Returns(RepositoryMock.Object);
            unitOfWorkMock.Setup(m => m.Begin()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Commit()).Returns(Task.CompletedTask);
            unitOfWorkMock.Setup(m => m.Rollback()).Returns(Task.CompletedTask);

            IProductService ServiceMock = new ProductService(unitOfWorkMock.Object, _mapper, _memoryCache, RepositoryStatusMock.Object, RepositoryDiscountMock.Object);

            //ACTIONS
            var testAdd = await ServiceMock.DeleteAsync(testId);
            //ASSERTIONS
            testAdd.Should().BeNull();
        }
    }
}
