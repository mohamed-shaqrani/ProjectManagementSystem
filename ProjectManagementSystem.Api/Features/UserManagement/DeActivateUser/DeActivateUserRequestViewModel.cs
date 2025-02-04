using FluentValidation;

namespace ProjectManagementSystem.Api.Features.UserManagement.DeActivateUser;

public record DeActivateUserRequestViewModel(int UserId, bool DeActive);

public class DeActivateUserRequestViewModelValidator : AbstractValidator<DeActivateUserRequestViewModel>
{
    public DeActivateUserRequestViewModelValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0);
        RuleFor(x => x.DeActive).Equal(true);
    }
}