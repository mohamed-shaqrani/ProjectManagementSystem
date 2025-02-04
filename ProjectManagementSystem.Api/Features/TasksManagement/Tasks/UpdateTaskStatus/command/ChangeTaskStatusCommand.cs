using MediatR;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTaskStatus.command
{
    public record ChangeTaskStatusCommand(int taskid,ProjectTaskStatus Status): IRequest<RequestResult<bool>>;
    public class ChangeTaskStatusHandler : BaseRequestHandler<ChangeTaskStatusCommand, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ChangeTaskStatusHandler(IUnitOfWork unitOfWork, BaseRequestHandlerParam param) : base(param) 
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task<RequestResult<bool>> Handle(ChangeTaskStatusCommand command,CancellationToken cancellation) 
        {
            var repo = _unitOfWork.GetRepository<ProjectTask>();
            var any = await repo.AnyAsync(t=>t.Id == command.taskid);

            if (!any) 
            {
                return RequestResult<bool>.Failure(Response.ErrorCode.NotFound, "The task does not exist");
            }

            var task = new ProjectTask { Id = command.taskid, Status = command.Status };

            repo.SaveInclude(task,t=>t.Status);

           await _unitOfWork.SaveChangesAsync();
            return RequestResult<bool>.Success(true, "Task status changed successfully");


        } 
    }
}
