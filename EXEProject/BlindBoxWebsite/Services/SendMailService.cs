using BlindBoxWebsite.DTO.MailDTOs;
using BlindBoxWebsite.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using System.Net;
using MailKit.Security;

namespace BlindBoxWebsite.Services
{
    public class SendMailService : ISendMailService
    {
        private readonly IConfiguration _configuration;
        private readonly MailSettings mailSettings;
        private readonly ILogger<SendMailService> logger;

        public SendMailService(IConfiguration configuration, IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger)
        {
            _configuration = configuration;
            mailSettings = _mailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
        }

        public async Task SendMail(MailContent mailContent)
        {

            var email = new MimeMessage();
            email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            logger.LogInformation("send mail to " + mailContent.To);

        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            await SendMail(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = message
            });
        }
    }
}
