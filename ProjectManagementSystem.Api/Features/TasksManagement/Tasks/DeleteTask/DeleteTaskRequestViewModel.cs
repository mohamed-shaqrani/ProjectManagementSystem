using FluentValidation;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask
{
    public record DeleteTaskRequestViewModel(int TaskID);

    public class DeleteTaskRequestViewModelValidator : AbstractValidator<DeleteTaskRequestViewModel>
    {
        public DeleteTaskRequestViewModelValidator()
        {
            RuleFor(x => x.TaskID).NotEmpty();

        }
    }
}
