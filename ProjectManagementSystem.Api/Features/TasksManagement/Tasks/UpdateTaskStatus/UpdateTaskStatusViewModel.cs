using FluentValidation;
using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTaskStatus
{
    public class UpdateTaskStatusViewModel
    {
        public int TaskID {  get; set; }
        public ProjectTaskStatus NewStatus { get; set; }

    }

    public class UpdateTaskStatusViewModelValidator : AbstractValidator<UpdateTaskStatusViewModel>
    {
        public UpdateTaskStatusViewModelValidator()
        {
            RuleFor(x => x.TaskID).NotEmpty();
            RuleFor(x => x.NewStatus).NotNull().NotEmpty();
        }
    }
}
