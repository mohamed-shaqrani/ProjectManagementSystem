using FluentValidation;
using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;

public record AddProjectRequestViewModel(string Title, ProjectStatus Status);

public class AddProjectRequestViewModelValidator : AbstractValidator<AddProjectRequestViewModel>
{
    public AddProjectRequestViewModelValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Not empty");
    }
}