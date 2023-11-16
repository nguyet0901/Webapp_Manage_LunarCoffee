using Microsoft.AspNetCore.Mvc;

namespace MLunarCoffee.Controllers
{
    public class BlogTagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
