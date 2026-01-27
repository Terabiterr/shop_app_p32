using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Shop_app_p32.Controllers
{
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
        //HTTP METHOD GET
        //https://localhost:[port]/users/auth
        [HttpGet]
        public ViewResult Auth()
        {
            return View();
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
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return BadRequest("Auth e-mail or password are error ...");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string email,  string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Register Email or password error ...");
            }
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            await _userManager.CreateAsync(user, password);
            return Ok("User is regestered successfully ...");
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
