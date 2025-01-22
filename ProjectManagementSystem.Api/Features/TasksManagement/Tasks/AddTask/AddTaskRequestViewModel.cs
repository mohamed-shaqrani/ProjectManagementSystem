using FluentValidation;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask
{
    public record AddTaskRequestViewModel(string Title, string Description, ProjectTaskStatus Status, int UserID);

    public class AddTaskRequestViewModelValidator : AbstractValidator<AddTaskRequestViewModel>
    {
        public AddTaskRequestViewModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Not empty");
        }
    }
}
