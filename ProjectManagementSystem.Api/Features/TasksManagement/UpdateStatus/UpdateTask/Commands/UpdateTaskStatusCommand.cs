using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask.Queries;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.UpdateStatus.UpdateTask.Commands
{
    public record UpdateTaskStatusCommand(int TaskID, ProjectTaskStatus Status, bool IsAadmin, string email) : IRequest<RequestResult<bool>>;

    public class UpdateTaskStatusHandler : BaseRequestHandler<UpdateTaskStatusCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateTaskStatusHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork)
            : base(requestHandlerParam)
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


            var task = new ProjectTask
            {
                Id = request.TaskID,
                Status = request.Status

            };

            _unitOfWork.GetRepository<ProjectTask>().SaveInclude(
                task,
                x => x.Status
                );
            await _unitOfWork.SaveChangesAsync();
            return RequestResult<bool>.Success(default, "Success");
        }

        private async Task<RequestResult<bool>> ValidateRequest(UpdateTaskStatusCommand request)
        {

            var taskExist = await _mediator.Send(new IsTaskExistQuery(request.TaskID));
            if (!taskExist.Data)

                return RequestResult<bool>.Failure(ErrorCode.TaskNotExist, "Task is not exist");

            var sameOldStatus = await _unitOfWork.GetRepository<ProjectTask>().AnyAsync(a => a.Status == request.Status && a.Id == request.TaskID);
            if (sameOldStatus)
            {
                return RequestResult<bool>.Failure(ErrorCode.TaskNotExist, $"Task Status Already {Enum.GetName(typeof(ProjectTaskStatus), request.Status)}");
            }
            if (!request.IsAadmin)
            {

                var userId = _unitOfWork.GetRepository<ProjectTask>().GetAll().Where(a => a.Id == request.TaskID).Select(a => a.UserID).First();
                var doesTaskBelongToUser = await _unitOfWork.GetRepository<User>().AnyAsync(a => a.Id == userId && a.Email == request.email);
                if (!doesTaskBelongToUser)
                {
                    return RequestResult<bool>.Failure(ErrorCode.TaskDoesNotBelongToUser, message: $"Task Status can not be updates as it does not belong to the user");

                }
            }

            return RequestResult<bool>.Success(default, "Success");
        }
    }
}
