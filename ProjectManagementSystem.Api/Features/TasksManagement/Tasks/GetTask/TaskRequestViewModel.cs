using FluentValidation;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask
{
    public record TaskRequestViewModel();
    public class TaskRequestViewModelValidator : AbstractValidator<TaskRequestViewModel>
    {
        public TaskRequestViewModelValidator() { }
    }
}
