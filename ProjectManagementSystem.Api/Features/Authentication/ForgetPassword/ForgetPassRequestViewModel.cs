using FluentValidation;

namespace ProjectManagementSystem.Api.Features.Authentication.ForgetPassword
{
    public record ForgetPassRequestViewModel(string Email);
    public class ForgetPassRequestViewModelValidator : AbstractValidator<ForgetPassRequestViewModel>
    {
        public ForgetPassRequestViewModelValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Not empty");
        }
    }
}