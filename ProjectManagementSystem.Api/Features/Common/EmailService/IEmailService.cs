namespace ProjectManagementSystem.Api.Features.Common.EmailService
{
    public interface IEmailServices
    {
        void SendEmail(string to, string subject, string body);
    }
}
