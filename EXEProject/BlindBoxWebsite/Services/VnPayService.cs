using BlindBoxWebsite.Heplers;
using BlindBoxWebsite.Interfaces;
using BlindBoxWebsite.Models;
using BlindBoxWebsite.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BlindBoxWebsite.Services
{
    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly BlindBoxContext _context;

        public VnPayService(IConfiguration config, BlindBoxContext context)
        {
            _configuration = config;
            _context = context;
        }

        public string CreatePaymentUrl(HttpContext context, VnPayRequestModel model)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]);
            var tick = DateTime.Now.Ticks.ToString();
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var pay = new VnPayLibrary();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            pay.AddRequestData("vnp_Version", _configuration["Vnpay:Version"]);
            pay.AddRequestData("vnp_Command", _configuration["Vnpay:Command"]);
            pay.AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]);
            pay.AddRequestData("vnp_Amount", ((int)model.Amount *100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]);
            pay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            pay.AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]);
            pay.AddRequestData("vnp_OrderInfo", $"{model.FullName} {model.OrderDescription} {model.Amount}");
            pay.AddRequestData("vnp_OrderType", model.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", tick);

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
                using (var context = new BlindBoxContext())
                {
                    int.TryParse(response.OrderId, out int orderId);

                    var order = context.Orders.FirstOrDefault(o => o.OrderId == orderId);
                    if (order != null)
                    {
                        order.Status = "Completed";
                        context.Orders.Update(order);
                    }

                    var payment = context.Payments.FirstOrDefault(p => p.OrderId == orderId);
                    if (payment != null)
                    {
                        payment.Status = "Success";
                        payment.CreatedAt = DateTime.Now;
                        context.Payments.Update(payment);
                    }

                    context.SaveChanges();
                }
            }
            else
            {
                using (var context = new BlindBoxContext())
                {
                    int.TryParse(response.OrderId, out int orderId);

                    var order = context.Orders.FirstOrDefault(o => o.OrderId == orderId);
                    if (order != null)
                    {
                        order.Status = "Failed";
                        context.Orders.Update(order);
                    }

                    var payment = context.Payments.FirstOrDefault(p => p.OrderId == orderId);
                    if (payment != null)
                    {
                        payment.Status = "Failed";
                        payment.CreatedAt = DateTime.Now;
                        context.Payments.Update(payment);
                    }

                    context.SaveChanges();
                }
            }
            return response;
        }
    }
}
