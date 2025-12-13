using Microsoft.AspNetCore.Mvc;

namespace Shop_app_p32.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
