using BlindBoxWebsite.Models;
using BlindBoxWebsite.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlindBoxWebsite.Services
{
    public class CheckoutService
    {
        private readonly BlindBoxContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckoutService(BlindBoxContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public OrderInfo MapToOrderInfo(VnPayRequestModel model, int orderId)
        {
            return new OrderInfo
            {
                Email = model.Email,
                FullName = model.FullName,
                City = model.City,
                District = model.District,
                Address = model.Address,
                PhoneNumber = model.Phone,
                OrderId = orderId,
                Order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId),
            };
        }

        private int GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext.Session.GetString("UserId");

            return int.Parse(userIdString);
        }
    }
}
