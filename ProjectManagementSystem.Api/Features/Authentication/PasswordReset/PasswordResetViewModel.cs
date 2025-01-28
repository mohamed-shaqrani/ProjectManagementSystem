using FluentValidation;

namespace ProjectManagementSystem.Api.Features.Authentication.PasswordReset;

public record PasswordResetViewModel(string Email, string NewPassword, string OTP);

public class PasswordResetViewModelValidator : AbstractValidator<PasswordResetViewModel>
{
    public PasswordResetViewModelValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.NewPassword).NotEmpty()
                                .MinimumLength(7).WithMessage("Password must be at least 7 characters long.")
                                .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character.")
                                .Must(ContainCapitalLetter).WithMessage("Password must contain at least one capital letter.")
                                .Must(ContainSmallLetter).WithMessage("Password must contain at least one small letter.");
    }
    private bool ContainSpecialCharacter(string arg)
    {
        var specialCharacters = "!@#$%^&*";
        return arg.Any(c => specialCharacters.Contains(c));
    }

    private bool ContainCapitalLetter(string password)
    {
        return password.Any(char.IsUpper);
    }

    private bool ContainSmallLetter(string password)
    {
        return password.Any(char.IsLower);
    }

}