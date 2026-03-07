using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]  // Авторизация с использованием схемы JWT Bearer
public class APICategoriesController : Controller
{
    private readonly ShopContext _context;

    public APICategoriesController(ShopContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid)
            return BadRequest("The model isn't valid ...");

        _context.Add(category);
        await _context.SaveChangesAsync();

        return Ok(category);
    }
}