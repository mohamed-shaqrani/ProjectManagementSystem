using FluentValidation;
using System.Text.RegularExpressions;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration;

public record RegisterViewModel(string Email, string Password, string Username, string phone, IFormFile? imageFile);

public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
{
    public RegisterViewModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().Must(IsValidEmail).EmailAddress();

        RuleFor(x => x.Password).NotEmpty()
            .MinimumLength(7).WithMessage("Password must be at least 7 characters long.")
            .Must(ContainSpecialCharacter).WithMessage("Password must contain at least one special character.")
            .Must(ContainCapitalLetter).WithMessage("Password must contain at least one capital letter.")
            .Must(ContainSmallLetter).WithMessage("Password must contain at least one small letter.");

        RuleFor(x => x.Username).NotEmpty().MinimumLength(2).WithMessage(" must be at least 2 characters long. ");
    }
    private bool ContainSpecialCharacter(string arg)
    {
        var specialCharacters = "!@#$%^&*";
        return arg.Any(c => specialCharacters.Contains(c));
    }
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^(?!.*\.\.)(?!.*\.$)(?!.*\.-)(?!.*-\.)([a-zA-Z0-9._%+-]+)@([a-zA-Z0-9.-]+)\.([a-zA-Z]{2,})$";
        return Regex.IsMatch(email, pattern);
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