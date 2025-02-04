using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.Projects.Queries;
using ProjectManagementSystem.Api.Features.Common.Users.Queries;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask.Queries;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTask.Commands
{
    public record UpdateTaskCommand(int TaskID, string Title, string Description, ProjectTaskStatus Status, int UserID, int ProjectID) :IRequest<RequestResult<bool>>;

    public class UpdateTaskCommandHandler : BaseRequestHandler<UpdateTaskCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateTaskCommandHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork)
            : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
        }
        public override async Task<RequestResult<bool>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var response = await ValidateRequest(request);
            if (!response.IsSuccess)
            {
                return response;
            }

            var task = new ProjectTask
            {
                Id = request.TaskID,
                UpdatedAt = DateTime.UtcNow,
                Title = request.Title,
                Description = request.Description,
                ProjectId = request.ProjectID,
                UserID = request.UserID,
                Status = request.Status

            };

            _unitOfWork.GetRepository<ProjectTask>().SaveInclude(
                task,
                x=>x.UpdatedAt,
                x=>x.Title,
                x=>x.Description,
                x=>x.Status,          
                x=>x.ProjectId,
                x=>x.UserID
                );
            await _unitOfWork.SaveChangesAsync();
            return RequestResult<bool>.Success(default, "Success");
        }

        private async Task<RequestResult<bool>> ValidateRequest(UpdateTaskCommand request)
        {
       
            var taskExist = await _mediator.Send(new IsTaskExistQuery(request.TaskID));
            if (!taskExist.Data)
            {
                return RequestResult<bool>.Failure(ErrorCode.TaskNotExist, "Task is not exist");
            }
            var userExist = await _mediator.Send(new IsUserExistQuery(request.UserID));
            if (!userExist)
            {
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
            }
            var projectExist = await _mediator.Send(new IsProjectExistQuery(request.ProjectID));
            if (!projectExist.Data)
            {
                return RequestResult<bool>.Failure(ErrorCode.ProjectNotExist, "Project not found");
            }
            if (!Enum.IsDefined(typeof(ProjectTaskStatus), request.Status))
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidTaskStatus, "Task status value is invalid");
            }

            return RequestResult<bool>.Success(default, "Success");
        }
    }
}
