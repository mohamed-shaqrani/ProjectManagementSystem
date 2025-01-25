using FluentValidation;

namespace ProjectManagementSystem.Api.Features.Authentication.ConfirmAccount;

public record ConfirmAccountViewModel(string Email, int Code);

public class RegisterViewModelValidator : AbstractValidator<ConfirmAccountViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Not empty");
    }
}