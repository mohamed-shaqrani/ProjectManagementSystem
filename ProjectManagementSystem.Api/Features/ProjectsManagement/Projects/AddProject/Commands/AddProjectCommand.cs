using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Authentication.UserRoles.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.GettingUserId;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;

public record AddProjectCommand(string Title) : IRequest<RequestResult<bool>>;
public class AddProjectHandler : BaseRequestHandler<AddProjectCommand, RequestResult<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGettingUserIdService _gettingUserIdService;

    public AddProjectHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork, IGettingUserIdService gettingUserIdService) : base(param)
    {
        _unitOfWork = unitOfWork;
        _gettingUserIdService = gettingUserIdService;
    }
    public override async Task<RequestResult<bool>> Handle(AddProjectCommand request, CancellationToken cancellationToken)
    {
        var project = new Project
        {
            CreatedAt = DateTime.UtcNow,
            Title = request.Title,
            Status = ProjectStatus.Completed,
        };
        var userid = await _gettingUserIdService.GettingUserId();
        var repo =  _unitOfWork.GetRepository<Project>();
        await repo.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();
        var save =  await _mediator.Send(new UserRolesCommand(project.Id, userid, Role.Admin));
        if (!save.IsSuccess) 
        {
            repo.ClearedTrackedChanges(project);
            return RequestResult<bool>.Failure(Response.ErrorCode.ProjectExist, "Project role already exist");
        }
        
        return RequestResult<bool>.Success(default, "Success");
    }
}
