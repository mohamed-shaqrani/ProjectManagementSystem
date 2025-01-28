using FluentValidation;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration;

public record RegisterViewModel(string Email, string Password, string Username, string phone, IFormFile imageFile);

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();

        RuleFor(x => x.Password).NotEmpty()
            .MinimumLength(7).WithMessage("Password must be at least 7 characters long.")
            .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character.")
            .Must(ContainCapitalLetter).WithMessage("Password must contain at least one capital letter.")
            .Must(ContainSmallLetter).WithMessage("Password must contain at least one small letter.");

        RuleFor(x => x.Username).NotEmpty().MinimumLength(2).WithMessage(" must be at least 2 characters long. ");
        RuleFor(x => x.imageFile.Length > 0);
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