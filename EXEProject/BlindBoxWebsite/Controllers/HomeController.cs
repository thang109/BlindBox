using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using BlindBoxWebsite.Models.Enums;
using BlindBoxWebsite.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlindBoxWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            // L?y userId t? HttpContext.Session
            var userIdString = HttpContext.Session.GetString("UserId");

            // Kh?i t?o bi?n userId
            int userId;

            // Ki?m tra v� chuy?n ??i gi� tr? userIdString th�nh s? nguy�n
            bool isUserIdValid = int.TryParse(userIdString, out userId);

            // Ki?m tra n?u userId h?p l? v� kh�ng null
            if (isUserIdValid)
            {
                var user = _userRepository.GetUserById(userId);

                // Ki?m tra n?u ng??i d�ng ?� ??ng nh?p
                bool isUserLoggedIn = user != null;

                // Thi?t l?p c�c ViewBag
                ViewBag.IsUserLoggedIn = isUserLoggedIn;

                if (isUserLoggedIn)
                {
                    ViewBag.UserName = HttpContext.Session.GetString("UserName");
                    ViewBag.Role = user.Role == UserRole.Admin ? "Admin" : "User";
                }
            }
            else
            {
                // N?u userId kh�ng h?p l?, x? l� ph� h?p (v� d?: ??ng xu?t ho?c th�ng b�o l?i)
                ViewBag.IsUserLoggedIn = false;
            }

            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
