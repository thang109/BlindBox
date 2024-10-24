using BlindBoxWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlindBoxWebsite.Controllers
{
    public class AdminController : Controller
    {
        private readonly BlindBoxContext _context;

        public AdminController(BlindBoxContext context)
        {
            _context = context;
        }

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
            var blindbox = _context.BlindBoxes.ToList();
            return View(blindbox);
        }

        public IActionResult UserManagement()
        {
            var user = _context.Users.ToList();
            return View(user);
        }
    }
}
