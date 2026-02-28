using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_app_p32.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]  // Авторизация с использованием схемы JWT Bearer
public class APICartController : Controller
{
    private readonly ShopDbContext _context;
    private readonly UserManager<ShopUser> _userManager;

    public APICartController(
        ShopDbContext context,
        UserManager<ShopUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        return Ok(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId)
    {
        var user = await _userManager.GetUserAsync(User);

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (cart == null)
        {
            cart = new Cart { UserId = user.Id };
            _context.Carts.Add(cart);
        }

        var existingItem = cart.Items
            .FirstOrDefault(i => i.ProductId == productId);

        if (existingItem != null)
            existingItem.Quantity++;
        else
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = 1
            });

        await _context.SaveChangesAsync();

        return Ok(cart);
    }
}