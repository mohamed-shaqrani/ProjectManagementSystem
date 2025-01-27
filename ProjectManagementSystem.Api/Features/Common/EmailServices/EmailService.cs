using MailKit.Net.Smtp;
using MimeKit;
using ProjectManagementSystem.Api.Config;


namespace ProjectManagementSystem.Api.Features.Common.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailService(EmailConfiguration emailConfig)
        {
            //_emailConfig = emailConfig;
            _emailConfig = new EmailConfiguration();
            _emailConfig.From = "userName@gmail.com";
            _emailConfig.SmtpServer = "smtp.gmail.com";
            _emailConfig.Port = 465;
            _emailConfig.UserName = "userName@gmail.com";
            _emailConfig.Password = "*************";
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
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
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
