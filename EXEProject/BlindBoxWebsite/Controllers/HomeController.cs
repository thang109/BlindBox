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
            var userIdString = HttpContext.Session.GetString("UserId");

            int userId;

            bool isUserIdValid = int.TryParse(userIdString, out userId);

            if (isUserIdValid)
            {
                var user = _userRepository.GetUserById(userId);

                bool isUserLoggedIn = user != null;

                ViewBag.IsUserLoggedIn = isUserLoggedIn;

                if (isUserLoggedIn)
                {
                    ViewBag.UserName = HttpContext.Session.GetString("UserName");
                    ViewBag.Role = user.Role == UserRole.Admin ? "Admin" : "User";
                }
            }
            else
            {
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
