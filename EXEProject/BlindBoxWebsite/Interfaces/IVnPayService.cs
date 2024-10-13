using BlindBoxWebsite.ViewModels;

namespace BlindBoxWebsite.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPayRequestModel model, int orderId);
        VnPayResponseModel PaymentExecute(IQueryCollection collections);
    }
}
