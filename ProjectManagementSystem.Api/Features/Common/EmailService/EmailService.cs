using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using ProjectManagementSystem.Api.Config;
using ProjectManagementSystem.Api.Features.Common.EmailService;


namespace ProjectManagementSystem.Api.Features.Common.EmailServices
{
    public class EmailService : IEmailServices
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            //_emailConfig = emailConfig;
            _emailConfig = emailConfig.Value;
        }

        public void SendEmail(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            if (string.IsNullOrEmpty(_emailConfig.From))
            {
                throw new InvalidOperationException("Sender email address is not configured.");
            }
            emailMessage.From.Add(new MailboxAddress("Support", _emailConfig.From));
            emailMessage.To.Add(new MailboxAddress("reciver", to));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            Send(emailMessage);
        }


        private void Send(MimeMessage mailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.CheckCertificateRevocation = false;
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

    }
}