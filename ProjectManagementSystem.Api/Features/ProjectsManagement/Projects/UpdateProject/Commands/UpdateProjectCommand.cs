using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;

public record UpdateProjectCommand(string Title,int Id) : IRequest<RequestResult<bool>>;
public class UpdateProjectHandler : BaseRequestHandler<UpdateProjectCommand, RequestResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjectHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
    {
        _unitOfWork = unitOfWork;
    }
    public override async Task<RequestResult<bool>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var checkProject = await _unitOfWork.GetRepository<Project>().AnyAsync(x => x.Title == request.Title && x.Id != request.Id);

        if (checkProject)
            return RequestResult<bool>.Failure(ErrorCode.ProjectExist, "Project with the same title already exists");


        var project = new Project
        {
            Id = request.Id,
            UpdatedAt = DateTime.UtcNow,
            Title = request.Title,
        };
         _unitOfWork.GetRepository<Project>().SaveInclude(project,a=>a.Title,a=>a.UpdatedAt);
        await _unitOfWork.SaveChangesAsync();
        return RequestResult<bool>.Success(default, "Success");
    }
}
