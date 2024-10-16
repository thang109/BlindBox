using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using BlindBoxWebsite.Services;
using BlindBoxWebsite.ViewModels;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IProductRepository _productRepository;
        private readonly CheckoutService _checkoutService;

        public PaymentController(BlindBoxContext context, IVnPayService vnPayService, ISendMailService sendMailService, IProductRepository productRepository, CheckoutService checkoutService)
        {
            _context = context;
            _vnPayService = vnPayService;
            _sendMailService = sendMailService;
            _productRepository = productRepository;
            _checkoutService = checkoutService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Checkout(decimal price, string imgUrl, string title)
        {
            ViewBag.Price = price;
            ViewBag.RawPrice = string.Format("{0:N0}", price);
            ViewBag.ImgUrl = imgUrl;
            ViewBag.Title = title;

            bool isUserLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            if (isUserLoggedIn)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }
            return View();
        }

        public IActionResult CheckoutBlindBox(int blindBoxId, decimal price, string name, string imageUrl, string description)
        {

            decimal discount = price * 10 / 100;
            decimal totalPrice = price - discount;

            bool isUserLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            if (isUserLoggedIn)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
            
            ViewBag.BlindBoxId = blindBoxId;
            ViewBag.Price = totalPrice;
            ViewBag.RawPrice = string.Format("{0:N0}", price);
            ViewBag.Discount = string.Format("{0:N0}", discount);
            ViewBag.TotalPrice = string.Format("{0:N0}", totalPrice);
            ViewBag.ImgUrl = imageUrl;
            ViewBag.Name = name;
            ViewBag.Description = description;

            if (blindBoxId == null || price == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(imageUrl) || string.IsNullOrEmpty(description))
            {
                TempData["ItemCheckout"] = "Vui lòng chọn sản phẩm trước khi thanh toán!";
                return RedirectToAction("BlindBoxGift", "Product");
            }
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

        public async Task<IActionResult> CreatePaymentUrl(VnPayRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("CheckoutBlindBox", model);
                }

                var userIdString = HttpContext.Session.GetString("UserId");
                if (!int.TryParse(userIdString, out int userId))
                {
                    return RedirectToAction("SignIn", "Account");
                }

                var newOrder = new Order()
                {
                    UserId = userId,
                    TotalPrice = (decimal)model.Amount,
                    Status = "Pending",
                    CreatedAt = model.CreateDate,
                    UpdatedAt = DateTime.Now,
                };
                newOrder.OrderId = await _productRepository.AddNewOrder(newOrder);

                var orderDetail = new OrderItem()
                {
                    OrderId = newOrder.OrderId,
                    BlindBoxId = model.BlindBoxId,
                    Quantity = 1,
                    Price = (decimal)model.Amount,
                    CreatedAt = model.CreateDate,
                    UpdatedAt = DateTime.Now,
                };
                await _productRepository.AddNewOrderItem(orderDetail);

                var payment = new Payment()
                {
                    OrderId = newOrder.OrderId,
                    PaymentMethod = model.PaymentType,
                    Amount = (decimal)model.Amount,
                    Status = "Pending",
                    CreatedAt = model.CreateDate,
                    UpdatedAt = DateTime.Now,
                };
                await _productRepository.AddNewPayment(payment);

                var orderInfo = _checkoutService.MapToOrderInfo(model, newOrder.OrderId);
                await _productRepository.AddNewOrderInfo(orderInfo);

                if (model.PaymentType == "VNPAY")
                {
                    var url = _vnPayService.CreatePaymentUrl(HttpContext, model, newOrder.OrderId);
                    return Redirect(url);
                }
                else if (model.PaymentType == "COD")
                {
                    newOrder.Status = "Confirmed";
                    _productRepository.UpdateOrder(newOrder);

                    payment.Status = "Paid";
                    _productRepository.UpdatePayment(payment);

                    var stockUpdated = _productRepository.UpdateStockBlindBox(orderDetail.BlindBoxId, orderDetail.Quantity);
                    if (!stockUpdated)
                    {
                        TempData["OutOfStock"] = "Sản phẩm hiện hết hàng! Hãy tham khảo các mẫu khác.";
                        return RedirectToAction("CheckoutBlindBox");
                    }

                    return RedirectToAction("PaymentSuccess");
                }

                return RedirectToAction("CheckoutBlindBox");
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
