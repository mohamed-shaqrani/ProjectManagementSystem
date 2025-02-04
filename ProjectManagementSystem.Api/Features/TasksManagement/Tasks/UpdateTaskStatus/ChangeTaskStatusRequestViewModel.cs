using FluentValidation;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTaskStatus
{
    public record ChangeTaskStatusRequestViewModel(int projectid, int taskid);
    public class ChangeTaskStatusRequestViewModelValidator : AbstractValidator<ChangeTaskStatusRequestViewModel>
    {
        public ChangeTaskStatusRequestViewModelValidator() 
        {
            this.RuleFor(r => r.taskid).GreaterThan(0);
        }
    }
}
