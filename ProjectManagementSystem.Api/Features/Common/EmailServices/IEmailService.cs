namespace ProjectManagementSystem.Api.Features.Common.EmailServices
{
    public interface IEmailService
    {
        void SendEmail(string to, string subject, string body);
    }
}
