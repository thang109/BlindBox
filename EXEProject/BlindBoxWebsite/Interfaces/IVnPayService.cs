using BlindBoxWebsite.ViewModels;

namespace BlindBoxWebsite.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPayRequestModel model);
        VnPayResponseModel PaymentExcute(IQueryCollection collections);
    }
}
