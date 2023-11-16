using Microsoft.AspNetCore.Mvc;

namespace MLunarCoffee.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
