using FluentAssertions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using Tekton.API.Controllers;
using Tekton.Domain.Entities;
using Tekton.Service.DTO;
using Tekton.Service.Interfaces;
using Tekton.UnitTests.Fixtures;

namespace Tekton.UnitTests.Systems.Controllers;

public class TestProductsController
{
    [Fact]
    public async Task Get_OnSuccess_ReturnsCode200()
    {
        // SETUP
        long testId = 1;
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.GetByIdAsync(testId))
            .ReturnsAsync(new ProductDTO());
        var systemUnderTest = new ProductsController(MockProductService.Object);
        // ACTIONS
        var result =(OkObjectResult) await systemUnderTest.GetById(testId);
        //ASSERTIONS
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }
    [Fact]
    public async Task Get_WhenNotFound_ReturnsCode404()
    {
        // SETUP
        long testId = 1;
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.GetByIdAsync(testId))
            .ReturnsAsync(() => null);
        var systemUnderTest = new ProductsController(MockProductService.Object);
        // ACTIONS
        var result = (NotFoundObjectResult) await systemUnderTest.GetById(testId);
        //ASSERTIONS
        result.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task Get_OnSuccess_InvokesProductService() {
        // SETUP
        long testId = 1;
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.GetByIdAsync(testId))
            .ReturnsAsync(ProductsFixture.GetTestProductDTOs()[0]);
        var systemUnderTest = new ProductsController(MockProductService.Object);

        // ACTIONS
        var result = (OkObjectResult)await systemUnderTest.GetById(testId);
        //ASSERTIONS
        MockProductService.Verify(
            service =>  service.GetByIdAsync(1),
            Times.Once()
        );
    }
    [Fact]
    public async Task Get_OnSuccess_FinalPriceIsValid()
    {
        // SETUP
        long testId = 2;
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.GetByIdAsync(testId))
            .ReturnsAsync(ProductsFixture.GetTestProductDTOs()[1]);
        var systemUnderTest = new ProductsController(MockProductService.Object);

        // ACTIONS
        var result = (OkObjectResult)await systemUnderTest.GetById(testId);
        //ASSERTIONS
        var product = result.Value as ProductDTO;
        var finalPrice = product!.Price * (100 - product.Discount) / 100;
        product.FinalPrice.Should().Be(finalPrice);
    }

    [Fact]
    public async Task GetAll_OnSuccess_ReturnsProductList()
    {
        // SETUP
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(ProductsFixture.GetTestProductDTOs());
        var systemUnderTest = new ProductsController(MockProductService.Object);

        // ACTIONS
        var result = await systemUnderTest.GetAll();
        //ASSERTIONS
        result.Should().BeOfType<OkObjectResult>();
        var objResult = (OkObjectResult) result;
        objResult.Value.Should().BeOfType<List<ProductDTO>>();
    }

    [Fact]
    public async Task GetAll_OnSuccess_ReturnsCode200()
    {
        // SETUP
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(ProductsFixture.GetTestProductDTOs());
        var systemUnderTest = new ProductsController(MockProductService.Object);
        // ACTIONS
        var result = (OkObjectResult)await systemUnderTest.GetAll();
        //ASSERTIONS
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetAll_WhenNotFound_ReturnsCode404()
    {
        // SETUP
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(new List<ProductDTO>());
        var systemUnderTest = new ProductsController(MockProductService.Object);

        // ACTIONS
        var result = await systemUnderTest.GetAll();
        //ASSERTIONS
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetAll_OnSuccess_InvokesProductService()
    {
        // SETUP
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(new List<ProductDTO>());
        var systemUnderTest = new ProductsController(MockProductService.Object);

        // ACTIONS
        var result = await systemUnderTest.GetAll();
        //ASSERTIONS
        MockProductService.Verify(
            service => service.GetAllAsync(),
            Times.Once()
        );
    }

    [Fact]
    public async Task Insert_OnSuccess_ReturnsCode200()
    {
        // SETUP
        var NewProductFixture = ProductsFixture.GetTestNewProductDTO();
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.AddAsync(NewProductFixture))
            .ReturnsAsync(new ProductDTO());
        var systemUnderTest = new ProductsController(MockProductService.Object);
        // ACTIONS
        var result = (OkObjectResult)await systemUnderTest.Insert(NewProductFixture);
        //ASSERTIONS
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Insert_OnSuccess_ReturnsProduct()
    {
        // SETUP
        var NewProductFixture = ProductsFixture.GetTestNewProductDTO();
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.AddAsync(NewProductFixture))
            .ReturnsAsync(new ProductDTO());
        var systemUnderTest = new ProductsController(MockProductService.Object);
        // ACTIONS
        var result = (OkObjectResult)await systemUnderTest.Insert(NewProductFixture);
        var objResult = (OkObjectResult)result;
        //ASSERTIONS
        objResult.Value.Should().BeOfType<ProductDTO>();
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Insert_OnSuccess_InvokesProductService()
    {
        // SETUP
        var NewProductFixture = ProductsFixture.GetTestNewProductDTO();
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.AddAsync(NewProductFixture))
            .ReturnsAsync(new ProductDTO());
        var systemUnderTest = new ProductsController(MockProductService.Object);

        // ACTIONS
        var result = (OkObjectResult)await systemUnderTest.Insert(NewProductFixture);
        //ASSERTIONS
        MockProductService.Verify(
            service => service.AddAsync(NewProductFixture),
            Times.Once()
        );
    }

    [Fact]
    public async Task Delete_OnSuccess_ReturnsCode200()
    {
        // SETUP
        long testId = 1;
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.DeleteAsync(testId))
            .ReturnsAsync(new ProductDTO());
        var systemUnderTest = new ProductsController(MockProductService.Object);
        // ACTIONS
        var result = (OkObjectResult)await systemUnderTest.Delete(testId);
        //ASSERTIONS
        result.StatusCode.Should().Be((int)HttpStatusCode.OK);
    }

    [Fact]
    public async Task Delete_OnSuccess_InvokesProductService()
    {
        // SETUP
        long testId = 1;
        var MockProductService = new Mock<IProductService>();
        MockProductService
            .Setup(service => service.DeleteAsync(testId))
            .ReturnsAsync(new ProductDTO());
        var systemUnderTest = new ProductsController(MockProductService.Object);

        // ACTIONS
        var result = (OkObjectResult)await systemUnderTest.Delete(testId);
        //ASSERTIONS
        MockProductService.Verify(
            service => service.DeleteAsync(testId),
            Times.Once()
        );
    }
}