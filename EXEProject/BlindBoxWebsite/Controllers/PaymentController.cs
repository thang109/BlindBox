using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using BlindBoxWebsite.Services;
using BlindBoxWebsite.ViewModels;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Asn1.X509;

namespace BlindBoxWebsite.Controllers
{
    public class PaymentController : Controller
    {
        private readonly BlindBoxContext _context;
        private readonly IVnPayService _vnPayService;
        private readonly ISendMailService _sendMailService;

        public PaymentController(BlindBoxContext context, IVnPayService vnPayService, ISendMailService sendMailService)
        {
            _context = context;
            _vnPayService = vnPayService;
            _sendMailService = sendMailService;
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

        public IActionResult CreatePaymentUrl(VnPayRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Checkout", model);
                }

                var url = _vnPayService.CreatePaymentUrl(HttpContext, model);

                return Redirect(url);
            }
            catch (Exception ex)
            {
                return RedirectToAction("PaymentFail");
            }
        }

        public async Task<IActionResult> PaymentCallback()
        {
            try
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
            catch (Exception ex)
            {
                return RedirectToAction("PaymentFail");
            }
        }
    }
}
