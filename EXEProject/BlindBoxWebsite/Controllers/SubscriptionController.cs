using Microsoft.AspNetCore.Mvc;

namespace BlindBoxWebsite.Controllers
{
    public class SubscriptionController : Controller
    {
        [HttpPost]
        public IActionResult SubmitSubscription(string email)
        {
            TempData["SubcribeSuccess"] = "Cảm ơn bạn đã đăng ký nhận thông báo.";

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Subscribe()
        {
            return View();
        }
    }
}
