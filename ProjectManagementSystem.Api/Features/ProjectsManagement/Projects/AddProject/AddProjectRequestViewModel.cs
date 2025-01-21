using FluentValidation;
using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;

public record AddProjectRequestViewModel(string Title, ProjectStatus Status);

public class AddRoomRequestViewModelValidator : AbstractValidator<AddProjectRequestViewModel>
{
    public AddRoomRequestViewModelValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Not empty");
    }
}