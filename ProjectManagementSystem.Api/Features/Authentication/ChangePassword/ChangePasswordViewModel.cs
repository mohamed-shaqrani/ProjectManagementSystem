using FluentValidation;

namespace ProjectManagementSystem.Api.Features.Authentication.ChangePassword;

public record ChangePasswordViewModel(int UserId, string OldPassword, string NewPassword);

public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
{
    public ChangePasswordViewModelValidator()
    {
    }
}