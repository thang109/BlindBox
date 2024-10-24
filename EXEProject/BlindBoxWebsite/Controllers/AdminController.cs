using BlindBoxWebsite.Models;
using BlindBoxWebsite.ViewModels;
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
            var totalUser = _context.Users.Count();
            var totalBlindBoxes = _context.BlindBoxes.Count();
            var totalPayment = _context.Orders
                           .Where(o => o.Status == "Completed")
                           .Sum(o => o.TotalPrice);

            ViewBag.TotalUser = totalUser;
            ViewBag.TotalBlindBoxes = totalBlindBoxes;
            ViewBag.TotalPayment = totalPayment;
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

        public IActionResult RevenueReport()
        {
            var totalRevenue = _context.Orders
                                   .Where(o => o.Status == "Confirmed")
                                   .Sum(o => o.TotalPrice);

            var totalOrders = _context.Orders.Count();

            var monthlyRevenue = _context.Orders
                                         .Where(o => o.Status == "Confirmed" && o.CreatedAt.HasValue)
                                         .GroupBy(o => o.CreatedAt.Value.Month)
                                         .Select(g => new MonthlyRevenue
                                         {
                                             Month = g.Key,
                                             TotalRevenue = g.Sum(o => o.TotalPrice)
                                         }).ToList();

            var revenueByCity = _context.OrderInfos
                                         .Join(_context.Orders,
                                               info => info.OrderId,
                                               order => order.OrderId,
                                               (info, order) => new { info.City, order.TotalPrice, order.Status })
                                         .Where(o => o.Status == "Confirmed" && o.TotalPrice != null)
                                         .GroupBy(o => o.City)
                                         .Select(g => new RevenueByCity
                                         {
                                             City = g.Key,
                                             TotalRevenue = g.Sum(o => o.TotalPrice)
                                         })
                                         .ToList();

            var orderInfo = _context.OrderInfos.ToList();
            var order = _context.Orders.ToList();

            var model = new RevenueReportViewModel
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                MonthlyRevenue = monthlyRevenue,
                RevenueByCity = revenueByCity,
                OrderInfo = orderInfo,
                Order = order
            };

            return View(model);
        }

        public IActionResult EditUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("UserManagement");
            }
            return View(user);
        }

        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("UserManagement");
        }

        public IActionResult EditBlindBoxes(int id)
        {
            var blindBox = _context.BlindBoxes.Find(id);
            if (blindBox == null)
            {
                return NotFound();
            }
            return View(blindBox);
        }

        [HttpPost]
        public IActionResult EditBlindBoxes(BlindBox blindBox)
        {
            if (ModelState.IsValid)
            {
                _context.BlindBoxes.Update(blindBox);
                _context.SaveChanges();
                return RedirectToAction("ProductManagement");
            }
            return View(blindBox);
        }

        public IActionResult DeleteBlindBoxes(int id)
        {
            var blindBox = _context.BlindBoxes.Find(id);
            if (blindBox != null)
            {
                _context.BlindBoxes.Remove(blindBox);
                _context.SaveChanges();
            }
            return RedirectToAction("ProductManagement");
        }

    }
}
