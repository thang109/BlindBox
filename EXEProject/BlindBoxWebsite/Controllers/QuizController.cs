using Microsoft.AspNetCore.Mvc;

namespace BlindBoxWebsite.Controllers
{
    public class QuizController : Controller
    {
        public IActionResult Quiz()
        {
            return View();
        }
        
        public IActionResult GiftSuggest()
        {
            return View();
        }
    }
}
