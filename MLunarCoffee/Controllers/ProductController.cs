using Microsoft.AspNetCore.Mvc;

namespace MLunarCoffee.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
