namespace ProjectManagementSystem.Api.Features.Common.EmailService
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
}
