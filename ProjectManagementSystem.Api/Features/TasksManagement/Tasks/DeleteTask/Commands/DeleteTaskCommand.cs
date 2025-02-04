using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask.Queries;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask.Commands
{
    public record DeleteTaskCommand(int TaskID): IRequest<RequestResult<bool>>;

    public class DeleteTaskCommandHandler : BaseRequestHandler<DeleteTaskCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteTaskCommandHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork)
            : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
        }


        public override async Task<RequestResult<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var response = await ValidateRequest(request);
            if (!response.IsSuccess)
            {
                return response;
            }

            var task = new ProjectTask { Id = request.TaskID };
            _unitOfWork.GetRepository<ProjectTask>().Delete(task);
            await _unitOfWork.SaveChangesAsync();

            return RequestResult<bool>.Success(default, "Success");
        }

        private async Task<RequestResult<bool>> ValidateRequest(DeleteTaskCommand request)
        {
           var TaskExist = await _mediator.Send(new IsTaskExistQuery(request.TaskID));
            if (!TaskExist.Data)
            {
                return RequestResult<bool>.Failure(ErrorCode.TaskNotExist, "Task not Exist");
            }
            return RequestResult<bool>.Success(default, "Success");
        }
    }
}
