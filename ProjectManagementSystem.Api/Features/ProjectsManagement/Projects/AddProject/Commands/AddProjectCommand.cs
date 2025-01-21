using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;

public record AddProjectCommand(string Title) : IRequest<RequestResult<bool>>;
public class AddProjectHandler : BaseRequestHandler<AddProjectCommand, RequestResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddProjectHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
    {
        _unitOfWork = unitOfWork;
    }
    public override async Task<RequestResult<bool>> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            CreatedAt = DateTime.UtcNow,
            Title = request.Title,
            Status = ProjectStatus.Completed,
        };
        await _unitOfWork.GetRepository<Project>().AddAsync(project);
        await _unitOfWork.SaveChangesAsync();
        return RequestResult<bool>.Success(default, "Success");
    }
}
