using FluentValidation;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTaskByStatus
{
    public record GroupedTasksRequestViewModel();
    public class GroupedTasksRequestViewModelValidator : AbstractValidator<GroupedTasksRequestViewModel>
    {
        public GroupedTasksRequestViewModelValidator() { }
    }
}
