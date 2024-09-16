using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using BlindBoxWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace BlindBoxWebsite.Controllers
{
    public class PaymentController : Controller
    {
        private readonly BlindBoxContext _context;
        private readonly IVnPayService _vnPayService;

        public PaymentController(BlindBoxContext context, IVnPayService vnPayService)
        {
            _context = context;
            _vnPayService = vnPayService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult CheckoutBlindBox()
        {
            return View();
        }

        public IActionResult PaymentSuccess(VnPayResponseModel model)
        {
            bool isUserLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            if (isUserLoggedIn)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }
            return View(model);
        }

        public IActionResult PaymentFail(VnPayResponseModel model)
        {
            bool isUserLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            if (isUserLoggedIn)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }
            return View(model);
        }

        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            //if (string.IsNullOrEmpty(model.Email) ||
            //    string.IsNullOrEmpty(model.FullName) ||
            //    string.IsNullOrEmpty(model.Phone))
            //{
            //    return RedirectToAction("Checkout", new { error = "Vui lòng điền đầy đủ thông tin" });
            //}
            var url = _vnPayService.CreatePaymentUrl(HttpContext, model);

            return Redirect(url);
        }

        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response.Success) 
            {
                return RedirectToAction("PaymentSuccess");
            }
            else
            {
                return RedirectToAction("PaymentFail");
            }
        }
    }
}
