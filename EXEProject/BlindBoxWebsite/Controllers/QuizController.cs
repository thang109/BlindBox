using BlindBoxWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlindBoxWebsite.Controllers
{
    public class QuizController : Controller
    {
        private readonly BlindBoxContext _context;

        public QuizController(BlindBoxContext context)
        {
            _context = context;
        }

        public IActionResult Quiz()
        {
            bool isUserLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            if (isUserLoggedIn)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }
            return View();
        }
        
        public IActionResult GiftSuggest()
        {
            bool isUserLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            if (isUserLoggedIn)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }

            var blindBoxGift = _context.BlindBoxes.ToList(); 
            return View(blindBoxGift);
        }
    }
}
