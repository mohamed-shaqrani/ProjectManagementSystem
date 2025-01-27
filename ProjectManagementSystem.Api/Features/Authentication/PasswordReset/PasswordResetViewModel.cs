using FluentValidation;

namespace ProjectManagementSystem.Api.Features.Authentication.ResetPassword;

public record PasswordResetViewModel(string Email, string NewPassword, string OTP);

public class PasswordResetViewModelValidator : AbstractValidator<PasswordResetViewModel>
{
    public PasswordResetViewModelValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .WithMessage("Not empty");

        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Not empty");
        RuleFor(x => x.OTP).NotEmpty().WithMessage("Not empty");
    }
}