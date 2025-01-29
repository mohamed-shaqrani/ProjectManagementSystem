using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;

public record DeleteProjectCommand(int Id) : IRequest<RequestResult<bool>>;
public class DeleteProjectHandler : BaseRequestHandler<UpdateProjectCommand, RequestResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
    {
        _unitOfWork = unitOfWork;
    }
    public override async Task<RequestResult<bool>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var projectExists = await _unitOfWork.GetRepository<Project>().AnyAsync(x => x.Id == request.Id);

        if (!projectExists)

            return RequestResult<bool>.Failure(ErrorCode.ProjectDoesNotExist, "Project does not exist");

        var projectHasTasks = await _unitOfWork.GetRepository<ProjectTask>().AnyAsync(a => a.ProjectId == request.Id);

        if (projectHasTasks)
            return RequestResult<bool>.Failure(ErrorCode.ProjectHasTasks, "Project can not be removed as it has assgined tasks");

        var project = new Project
        {
            Id = request.Id,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = true,
        };
        _unitOfWork.GetRepository<Project>().SaveInclude(project, a => a.IsDeleted, a => a.UpdatedAt);
        await _unitOfWork.SaveChangesAsync();
        return RequestResult<bool>.Success(true, "Project has been deleted successfully ");
    }
}
