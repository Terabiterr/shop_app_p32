using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_app_p32.Models;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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

        Console.WriteLine($"Username = {user.UserName}");

        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == user.Id);

        return Ok(cart);
    }

    /*
     Знаходить користувача nskmrb не вказаний JWT Token
     */

    [HttpPost("{productId}")]
    public async Task<IActionResult> AddToCart(int productId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
            return Unauthorized();

        Console.WriteLine($"User Id: {user.Id}");

        var cart = await _context.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (cart == null)
        {
            cart = new Cart { UserId = user.Id };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
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