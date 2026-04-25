using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shop_app_p32.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shop_app_p32.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIUsersController : Controller
    {
        private readonly UserManager<ShopUser> _userManager;
        private readonly SignInManager<ShopUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public APIUsersController(UserManager<ShopUser> userManager, SignInManager<ShopUser> signInManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 🔍 шукаємо користувача по email
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            // 🔐 перевірка пароля
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,      // ⚠️ Identity працює через UserName
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (!result.Succeeded)
            {
                return BadRequest("Invalid email or password");
            }

            // 🎟️ генеруємо JWT
            var token = await GenerateJwtToken(user);

            return Ok(new
            {
                token = token,
                email = user.Email,
                userName = user.UserName
            });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 🔍 перевірка чи існує email
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists");
            }

            // 👤 створення користувача
            var user = new ShopUser
            {
                UserName = model.Username,
                Email = model.Email,
                EmailConfirmed = true
            };

            // 🔐 створення з паролем (Identity сам хешує!)
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new
                {
                    message = "User registered successfully"
                });
            }

            // ❌ якщо помилки (наприклад пароль слабкий)
            return BadRequest(result.Errors);
        }
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("The role name is empty ...");
            }
            var role_exists = await _roleManager.RoleExistsAsync(roleName);
            if (role_exists)
            {
                return BadRequest("The role name is exists ...");
            }
            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("AssingRole", "Users");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("assignrole")]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest("User id or rol name are empty ...");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User id is incorrect ...");
            }
            var role_exists = await _roleManager.RoleExistsAsync(roleName);
            if (!role_exists)
            {
                return BadRequest("Role name is incorrect ...");
            }
            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return BadRequest(result.Errors);
        }
        private async Task<string> GenerateJwtToken(ShopUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user); // 🟢 Отримуємо ролі

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            // 🟢 Додаємо ролі в claims
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
