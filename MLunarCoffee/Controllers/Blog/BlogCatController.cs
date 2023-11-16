using Microsoft.AspNetCore.Mvc;

namespace MLunarCoffee.Controllers
{
    public class BlogCatController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
