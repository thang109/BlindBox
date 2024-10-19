using BlindBoxWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlindBoxWebsite.Controllers
{
    public class ProductController : Controller
    {
        private readonly BlindBoxContext _context;

        public ProductController(BlindBoxContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BlindBox(int blindBoxId, string name, decimal price, string imageUrl, string description, int stock, string product)
        {
            bool isUserLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            if (isUserLoggedIn)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }

            ViewBag.BlindBoxId = blindBoxId;
            ViewBag.Name = name;
            ViewBag.Price = price;
            ViewBag.RawPrice = string.Format("{0:N0}", price);
            ViewBag.ImageUrl = imageUrl;
            ViewBag.Description = description;
            ViewBag.Stock = stock;
            ViewBag.Product = product ?? string.Empty;

            return View();
        }

        public IActionResult Gift()
        {
            bool isUserLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            if (isUserLoggedIn)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }
            return View();
        }

        public async Task<IActionResult> BlindBoxGift()
        {
            bool isUserLoggedIn = HttpContext.Session.GetString("UserId") != null;
            ViewBag.IsUserLoggedIn = isUserLoggedIn;
            if (isUserLoggedIn)
            {
                ViewBag.UserName = HttpContext.Session.GetString("UserName");
            }

            var blindBoxProducts = await _context.BlindBoxes.ToListAsync();

            return View(blindBoxProducts);
        }

        [HttpPost]
        public async Task<IActionResult> UploadProductImage(IFormFile file, int productId)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images/blindboxes", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var product = await _context.BlindBoxes.FindAsync(productId);
                if (product != null)
                {
                    product.ImageUrl = $"images/blindboxes/{file.FileName}";
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("BlindBoxGift");
        }

    }
}
