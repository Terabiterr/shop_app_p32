using Moq;
using Shop_app_p32.Controllers.API;
using Shop_app_p32.Services;

namespace Shop_app_p32.Controllers.Test
{
    public class ProductsControllerTests
    {
        private readonly Mock<IServiceProduct> _mockService;
        private readonly APIProductsController _controller;
        ProductsControllerTests()
        {
            _mockService = new Mock<IServiceProduct>();
            _controller = new APIProductsController(_mockService.Object);
        }
    }
}
