using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask.Queries
{
    public record IsTaskExistQuery(int TaskID):IRequest<RequestResult<bool>>;

    public class IsTaskExistQueryHandler : BaseRequestHandler<IsTaskExistQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public IsTaskExistQueryHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork)
            : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
        }
        public override async Task<RequestResult<bool>> Handle(IsTaskExistQuery request, CancellationToken cancellationToken)
        {
            var isExist = await _unitOfWork.GetRepository<ProjectTask>().AnyAsync(x => x.Id == request.TaskID && !x.IsDeleted);
            if (isExist)
            {
                return RequestResult<bool>.Success(true, "Task Exists correctly.");
            }
            return RequestResult<bool>.Success(false, "Task is not Exist.");
        }
    }
}
