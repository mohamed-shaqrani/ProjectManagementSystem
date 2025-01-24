namespace ProjectManagementSystem.Api.Features.Authentication
{
    public record LoginViewModel(string Email, string Password);

    public record RegisterViewModel(string Email, string Password,string Username);
   
}
