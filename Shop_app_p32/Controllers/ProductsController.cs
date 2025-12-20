using Microsoft.AspNetCore.Mvc;
using Shop_app_p32.Services;

namespace Shop_app_p32.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IServiceProduct _serviceProduct;
        public ProductsController(IServiceProduct serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }

        public async Task<IActionResult> Read()
        {
            var products = await _serviceProduct.GetAsync();
            return View(products);
        }
    }
}
