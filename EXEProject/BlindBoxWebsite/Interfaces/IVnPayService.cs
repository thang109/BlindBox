using BlindBoxWebsite.ViewModels;

namespace BlindBoxWebsite.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPayRequestModel model);
        VnPayResponseModel PaymentExecute(IQueryCollection collections);
    }
}
