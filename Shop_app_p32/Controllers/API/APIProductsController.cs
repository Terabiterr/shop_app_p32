using Microsoft.AspNetCore.Mvc;
using Shop_app_p32.Models;
using Shop_app_p32.Services;

namespace Shop_app_p32.Controllers.API
{
    //Додати Авторизацію через JWT Bearer
    //Додати реєстрацію
    [Route("api/[controller]")]
    [ApiController]
    public class APIProductsController : Controller
    {
        private readonly IServiceProduct _serviceProduct;
        public APIProductsController(IServiceProduct serviceProduct)
        {
            _serviceProduct = serviceProduct;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Json(new { status = 415 }));
            }
            await _serviceProduct.CreateAsync(product);
            return Ok(Json(product));
        }
        [HttpGet]
        public async Task<IActionResult> Read()
        {
            var products = await _serviceProduct.GetAsync();
            return Ok(Json(products));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Json(new { status = 415 }));
            }
            await _serviceProduct.UpdateAsync(id, product);
            return Ok(Json(product));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceProduct.DeleteAsync(id);
            if(result != null)
            {
                return Ok(Json(result));
            }
            return BadRequest(Json(new { status = 415 }));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _serviceProduct.GetByIdAsync(id);
            if (result != null)
            {
                return Ok(Json(result));
            }
            return BadRequest(Json(new { status = 415 }));
        }
    }
}
