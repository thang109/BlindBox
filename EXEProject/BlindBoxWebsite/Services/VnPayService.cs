using BlindBoxWebsite.Heplers;
using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using BlindBoxWebsite.ViewModels;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;

namespace BlindBoxWebsite.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly BlindBoxContext _context;
        private readonly IProductRepository _productRepository;

        public VnPayService(IConfiguration config, BlindBoxContext context, IProductRepository productRepository)
        {
            _configuration = config;
            _context = context;
            _productRepository = productRepository;
        }

        public string CreatePaymentUrl(HttpContext context, VnPayRequestModel model, int orderId)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var tick = DateTime.Now.Ticks.ToString();
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
            
            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount *100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.FullName} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.PaymentType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());

            var paymentUrl =
                pay.CreateRequestUrl(_configuration["Vnpay:BaseUrl"], _configuration["Vnpay:HashSecret"]);

            return paymentUrl;
        }

        public VnPayResponseModel PaymentExecute(IQueryCollection collections)
        {
            var pay = new VnPayLibrary();
            var response = pay.GetFullResponseData(collections, _configuration["Vnpay:HashSecret"]);
            if (response.Success)
            {
                int.TryParse(response.OrderId, out int orderId);
                using (var context = new BlindBoxContext())
                {
                    var payment = _context.Payments.FirstOrDefault(p => p.OrderId == orderId);
                    if (payment != null)
                    {
                        payment.Status = "Paid";
                        payment.UpdatedAt = DateTime.Now;
                        _context.Payments.Update(payment);
                    }

                    var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
                    if (order != null)
                    {
                        order.Status = "Paid";
                        order.UpdatedAt = DateTime.Now;
                        _context.Orders.Update(order);

                        var orderItems = _context.OrderItems.Where(oi => oi.OrderId == orderId).ToList();

                        foreach (var item in orderItems)
                        {
                            var blindBoxId = item.BlindBoxId;
                            _productRepository.UpdateStockBlindBox(blindBoxId, item.Quantity);
                        }
                    }

                    _context.SaveChanges();
                }
            }
            else
            {
                int.TryParse(response.OrderId, out int orderId);
                using (var context = new BlindBoxContext())
                {
                    var payment = _context.Payments.FirstOrDefault(p => p.OrderId == orderId);
                    if (payment != null)
                    {
                        payment.Status = "Fail";
                        payment.UpdatedAt = DateTime.Now;
                        _context.Payments.Update(payment);
                    }

                    var order = _context.Orders.FirstOrDefault(o => o.OrderId == orderId);
                    if (order != null)
                    {
                        order.Status = "Fail";
                        order.UpdatedAt = DateTime.Now;
                        _context.Orders.Update(order);
                    }

                    _context.SaveChanges();
                }
            }
            return response;
        }
    }
}
