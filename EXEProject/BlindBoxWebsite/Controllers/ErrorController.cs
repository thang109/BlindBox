using BlindBoxWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlindBoxWebsite.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var errorModel = new ErrorViewModel
            {
                StatusCode = statusCode,
                Message = statusCode == 404 ? "Không tìm thấy trang yêu cầu." :
                  statusCode == 500 ? "Đã xảy ra lỗi nội bộ. Vui lòng thử lại sau." :
                  "Có lỗi xảy ra. Vui lòng thử lại sau."
            };

            return View("Error", errorModel);
        }
    }
}
