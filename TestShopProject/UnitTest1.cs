using Moq;
using Shop_app_p32.Controllers.API;
using Shop_app_p32.Models;
using Shop_app_p32.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;

namespace Shop_app_p32.Tests
{
    public class APIProductsControllerTests
    {
        private readonly Mock<IServiceProduct> _mockService;
        private readonly APIProductsController _controller;
        private readonly ITestOutputHelper _output;

        public APIProductsControllerTests(ITestOutputHelper output)
        {
            _mockService = new Mock<IServiceProduct>();
            _controller = new APIProductsController(_mockService.Object);
            _output = output;
        }

        [Fact]
        public async Task Read_ReturnsOkResult_WithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Test Product" }
            };
            _mockService.Setup(s => s.GetAsync()).ReturnsAsync(products);

            _output.WriteLine("Test started ...");
            // Act
            var result = await _controller.Read();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _output.WriteLine("Test ended ...");
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = 155, Name = "Test Product" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetById(155);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        //[Fact]
        //public async Task GetById_ReturnsBadRequest_WhenProductNotFound()
        //{
        //    // Arrange
        //    _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Product)null);

        //    // Act
        //    var result = await _controller.GetById(99);

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}

        //[Fact]
        //public async Task Create_ReturnsOkResult_WhenModelIsValid()
        //{
        //    // Arrange
        //    var product = new Product { Id = 1, Name = "New Product" };
        //    _mockService.Setup(s => s.CreateAsync(product)).Returns((Task<Product>)Task.CompletedTask);

        //    // Act
        //    var result = await _controller.Create(product);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    Assert.NotNull(okResult.Value);
        //}

        //[Fact]
        //public async Task Delete_ReturnsOkResult_WhenDeleted()
        //{
        //    // Arrange
        //    var product = new Product { Id = 1, Name = "Deleted Product" };
        //    _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(product);

        //    // Act
        //    var result = await _controller.Delete(1);

        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    Assert.NotNull(okResult.Value);
        //}

        //[Fact]
        //public async Task Delete_ReturnsBadRequest_WhenNotFound()
        //{
        //    // Arrange
        //    _mockService.Setup(s => s.DeleteAsync(99)).ReturnsAsync((Product)null);

        //    // Act
        //    var result = await _controller.Delete(99);

        //    // Assert
        //    Assert.IsType<BadRequestObjectResult>(result);
        //}
    }
}
