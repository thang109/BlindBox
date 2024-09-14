using Microsoft.AspNetCore.Mvc;

namespace BlindBoxWebsite.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BlindBox()
        {
            return View();
        }

        public IActionResult Gift()
        {
            return View();
        }
    }
}
