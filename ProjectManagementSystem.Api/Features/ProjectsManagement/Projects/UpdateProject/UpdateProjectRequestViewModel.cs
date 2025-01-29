using FluentValidation;
using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;

public record UpdateProjectRequestViewModel(string Title, int Id);

public class UpdateProjectValidator : AbstractValidator<UpdateProjectRequestViewModel>
{
    public UpdateProjectValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Id).NotNull().GreaterThan(0);
    }
}