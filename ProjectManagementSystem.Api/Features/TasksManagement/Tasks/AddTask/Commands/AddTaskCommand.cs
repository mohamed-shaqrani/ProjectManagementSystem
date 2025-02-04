using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.Projects.Queries;
using ProjectManagementSystem.Api.Features.Common.Users.Queries;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask.Commands
{
    public record AddTaskCommand(string Title, string Description, ProjectTaskStatus Status, int UserID, int ProjectID) : IRequest<RequestResult<bool>>;

    public class AddTaskCommandHandler : BaseRequestHandler<AddTaskCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddTaskCommandHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork)
            : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task<RequestResult<bool>> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var response = await ValidateRequest(request);
            if (!response.IsSuccess)
            {
                return response;
            }

            var task = new ProjectTask
            {
                CreatedAt = DateTime.UtcNow,
                Title = request.Title,
                Status = request.Status,
                Description = request.Description,
                UserID = request.UserID,
                ProjectId = request.ProjectID
            };

            await _unitOfWork.GetRepository<ProjectTask>().AddAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return RequestResult<bool>.Success(default, "Success");
        }

        private async Task<RequestResult<bool>> ValidateRequest(AddTaskCommand request)
        {
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

            return RequestResult<bool>.Success(default, "Success");
        }

    }

}
