using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Shop_app_p32.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Auth(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Auth Email or password error ...");
            }
            var result = await _signInManager.PasswordSignInAsync(
                    email,
                    password,
                    isPersistent: true,
                    lockoutOnFailure: false
                );
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return BadRequest("Auth e-mail or password are error ...");
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] IdentityUser newUser)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new { result = $"Error: model state ..." });
            }
            var user = new IdentityUser
            {
                UserName = newUser.UserName,
                Email = newUser.Email,
                PasswordHash = newUser.PasswordHash,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result.Errors);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
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

        [HttpPost]
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
    }
}
