using BlindBoxWebsite.ViewModels;

namespace BlindBoxWebsite.Interfaces
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, PaymentInformationModel model);
        VnPayResponseModel PaymentExecute(IQueryCollection collections);
    }
}
