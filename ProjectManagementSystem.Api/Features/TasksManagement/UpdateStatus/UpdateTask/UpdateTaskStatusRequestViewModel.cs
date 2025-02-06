using FluentValidation;
using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.TasksManagement.UpdateStatus.UpdateTask
{
    public record UpdateTaskStatusRequestViewModel(int TaskID, ProjectTaskStatus Status);
    public class UpdateTaskStatusRequestViewModelValidator : AbstractValidator<UpdateTaskStatusRequestViewModel>
    {
        public UpdateTaskStatusRequestViewModelValidator()
        {
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.TaskID).GreaterThan(0);

        }
    }
}