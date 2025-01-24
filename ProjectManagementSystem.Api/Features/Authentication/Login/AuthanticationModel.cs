namespace ProjectManagementSystem.Api.Features.Authentication.Login
{
    public class AuthanticationModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
