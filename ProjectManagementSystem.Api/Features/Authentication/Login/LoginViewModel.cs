using FluentValidation;
using ProjectManagementSystem.Api.Features.Authentication.Registration;

namespace ProjectManagementSystem.Api.Features.Authentication.Login
{
    public record LoginViewModel(string Email, string Password);

    public class LoginViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public LoginViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Not empty");
        }
    }
}
