using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.Common.GettingUserId;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTaskStatus.command
{
    public record ChangeTaskStatusCommand(int taskid,ProjectTaskStatus Status): IRequest<RequestResult<bool>>;
    public class ChangeTaskStatusHandler : BaseRequestHandler<ChangeTaskStatusCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGettingUserIdService _gettingUserIdService;
        public ChangeTaskStatusHandler(IUnitOfWork unitOfWork, BaseRequestHandlerParam param, IGettingUserIdService gettingUserIdService) : base(param) 
        {
            _unitOfWork = unitOfWork;
            _gettingUserIdService = gettingUserIdService;
        }

        public override async Task<RequestResult<bool>> Handle(ChangeTaskStatusCommand command,CancellationToken cancellation) 
        {
            if (!await Validate(command)) 
            {
                return RequestResult<bool>.Failure(Response.ErrorCode.NotFound, "Task status unchanged");
            }
            var repo = _unitOfWork.GetRepository<ProjectTask>();

            var task = new ProjectTask { Id = command.taskid, Status = command.Status };

            repo.SaveInclude(task,t=>t.Status);

           await _unitOfWork.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Task status changed successfully");


        } 


        public async Task<bool> Validate(ChangeTaskStatusCommand command) 
        {
            var repo = _unitOfWork.GetRepository<ProjectTask>();
            var t = await repo.GetAll(t => t.Id == command.taskid).Select(t => new { t.ProjectId }).FirstOrDefaultAsync();

            if (t is null)
            {
                return false;
            }
            var userid = await _gettingUserIdService.GettingUserId();

            var Projectusersrepo = _unitOfWork.GetRepository<ProjectUserRoles>();

            var any = await Projectusersrepo.AnyAsync(pr => pr.UserId == userid && pr.ProjectId == t.ProjectId);

            if (!any) { return false; }

            return true;

        }
    }
}
