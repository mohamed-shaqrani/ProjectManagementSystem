using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Authentication.UserRoles.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.GettingUserId;
using ProjectManagementSystem.Api.Features.Common.Users.Queries;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask.Commands
{
    public record AddTaskCommand(string Title, string Description, ProjectTaskStatus Status) : BaseCommand, IRequest<RequestResult<bool>>;

    public class AddTaskCommandHandler : BaseRequestHandler<AddTaskCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGettingUserIdService _gettingUserIdService;
        public AddTaskCommandHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork, IGettingUserIdService gettingUserIdService)
            : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
            _gettingUserIdService = gettingUserIdService;
        }

        public override async Task<RequestResult<bool>> Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            var userid = await this._gettingUserIdService.GettingUserId();
            var response = await ValidateRequest(userid);
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
                UserID = userid,
                ProjectId = request.ProjectId,
                
            };

            await _unitOfWork.GetRepository<ProjectTask>().AddAsync(task);
            
            await _unitOfWork.SaveChangesAsync();
            return RequestResult<bool>.Success(default, "Success");
        }

        private async Task<RequestResult<bool>> ValidateRequest(int request)
        {
            var UserExist = await _mediator.Send(new IsUserExistQuery(request));
            if (!UserExist)
            {
                return RequestResult<bool>.Failure(ErrorCode.UserNotFound, "User not found");
            }

            return RequestResult<bool>.Success(default, "Success");
        }

    }

}
