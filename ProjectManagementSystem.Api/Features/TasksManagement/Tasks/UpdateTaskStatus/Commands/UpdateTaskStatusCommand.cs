using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask.Queries;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTaskStatus.Commands
{
    public record UpdateTaskStatusCommand(int TaskID, ProjectTaskStatus NewStatus): IRequest<RequestResult<bool>>;

    public class UpdateTaskStatusCommandHandler : BaseRequestHandler<UpdateTaskStatusCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskStatusCommandHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
        {
            _unitOfWork = unitOfWork;
        }
        public override async Task<RequestResult<bool>> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var response = await ValidateRequest(request);
            if (!response.IsSuccess)
            {
                return response;
            }
            var task = new ProjectTask {
                Id = request.TaskID,
                UpdatedAt = DateTime.UtcNow,
                Status = request.NewStatus
            };
            _unitOfWork.GetRepository<ProjectTask>().SaveInclude(task, t => t.Status, t => t.UpdatedAt);
            await _unitOfWork.SaveChangesAsync();
            return RequestResult<bool>.Success(default, "Success");

        }

        private async Task<RequestResult<bool>> ValidateRequest(UpdateTaskStatusCommand request)
        {
            var taskExist = await _mediator.Send(new IsTaskExistQuery(request.TaskID));
            if (!taskExist.Data)
            {
                return RequestResult<bool>.Failure(ErrorCode.TaskNotExist, " Task is not exist");
            }
            if(!Enum.IsDefined(typeof(ProjectTaskStatus), request.NewStatus))
            {
                return RequestResult<bool>.Failure(ErrorCode.InvalidTaskStatus, "Task status value is invalid");

            }
            return RequestResult<bool>.Success(default, "Success");

        }
    }
}
