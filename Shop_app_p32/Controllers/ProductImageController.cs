using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace Shop_app_p32.Controllers
{
    public class ProductImageController : Controller
    {
        private readonly ShopContext _context;
        public ProductImageController(ShopContext context)
        {
            _context = context; 
        }
        public async Task<IActionResult> AddImage()
        {
            return View();
        }
        [Authorize(Roles = "admin,moderator")]
        [HttpPost]
        public async Task<IActionResult> AddImage([Bind("Id,ImageUrl,ProductId")]ProductImage image)
        {
            try
            {
                if (image.ImageUrl != null && image.ProductId != null)
                {
                    await _context.ProductImages.AddAsync(image);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Read", "Products");
                }
                else
                {
                    Console.WriteLine($"Error bind ProductImage ...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
                return View(image);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteImage()
        {
            return View(); 
        }
        [Authorize(Roles = "admin,moderator")]
        [HttpPost]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var product_image = await _context.ProductImages.FindAsync(id);
            if(product_image != null)
            {
                _context.ProductImages.Remove(product_image);
                await _context.SaveChangesAsync();
                return RedirectToAction("Read", "Products");
            }
            return View(id);
        }
        //Додати views -> ProductImage ->
        //AddImage.cshtml, DeleteImage.cshtml
    }
}
