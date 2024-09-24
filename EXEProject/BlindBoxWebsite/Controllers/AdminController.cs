using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlindBoxWebsite.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Analytics()
        {
            return View();
        }

        public IActionResult ProductManagement()
        {
         
            return View();
        }

        public IActionResult UserManagement()
        {
            return View();
        }
    }
}
