using BlindBoxWebsite.DTO.MailDTOs;

namespace BlindBoxWebsite.Interfaces
{
    public interface ISendMailService
    {
        Task SendMail(MailContent mailContent);

        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
