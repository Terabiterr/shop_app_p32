using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop_app_p32.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly ShopContext _context;
        public ProductImageController(ShopContext context)
        {
            _context = context; 
        }
        [Authorize(Roles = "admin,moderator")]
        public async Task<IActionResult> AddImage([Bind("Id,ImageUrl,ProductId")]ProductImage image)
        {
            if(ModelState.IsValid)
            {
                await _context.ProductImages.AddAsync(image);
                return RedirectToAction("Read", "Products");
            }
            return View(image);
        }
    }
}
