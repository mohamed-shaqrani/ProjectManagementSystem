using FluentValidation;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;

public record DeleteProjectRequestViewModel(int Id);

public class DeleteProjectValidator : AbstractValidator<DeleteProjectRequestViewModel>
{
    public DeleteProjectValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);

    }
}