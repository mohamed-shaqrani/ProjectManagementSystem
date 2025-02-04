using FluentValidation;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTask
{
    public record UpdateTaskRequestViewModel(int TaskID , string Title, string Description, ProjectTaskStatus Status, int UserID, int ProjectID);
    public class UpdateTaskRequestViewModelValidator : AbstractValidator<UpdateTaskRequestViewModel>
    {
        public UpdateTaskRequestViewModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Not empty");
        }
    }
}