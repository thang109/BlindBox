using BlindBoxWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

            var randomProductsJson = HttpContext.Session.GetString("RandomProducts");

            List<BlindBox> randomProducts = string.IsNullOrEmpty(randomProductsJson)
                ? new List<BlindBox>()
                : JsonConvert.DeserializeObject<List<BlindBox>>(randomProductsJson);

            return View(randomProducts);
        }

        public IActionResult CompleteQuiz()
        {
            var allProducts = _context.BlindBoxes.ToList();

            var random = new Random();
            var randomProducts = allProducts.OrderBy(p => random.Next()).Take(2).ToList();

            HttpContext.Session.SetString("RandomProducts", Newtonsoft.Json.JsonConvert.SerializeObject(randomProducts));

            return RedirectToAction("GiftSuggest", "Quiz");
        }
    }
}
