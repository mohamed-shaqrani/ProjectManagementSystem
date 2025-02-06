using MediatR;
using PredicateExtensions;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.MappingProfiles;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;
using System.Linq.Expressions;


namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask.Queries
{
    public record GetTasksQuery(TaskParam TaskParam) : IRequest<RequestResult<PageList<TaskDTO>>>;

    public class GetTasksQueryHandler : BaseRequestHandler<GetTasksQuery, RequestResult<PageList<TaskDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTasksQueryHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
        {
            _unitOfWork = unitOfWork;
        }
        public override async Task<RequestResult<PageList<TaskDTO>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {

            var predicate = BuildPredicate(request.TaskParam);
            var query = _unitOfWork.GetRepository<ProjectTask>().GetAll(predicate).ProjectTo<TaskDTO>();
            var res = await PageList<TaskDTO>.CreateAsync(query, request.TaskParam.PageNumber, request.TaskParam.PageSize);

            return RequestResult<PageList<TaskDTO>>.Success(res, "Success");
        }
        private Expression<Func<ProjectTask, bool>> BuildPredicate(TaskParam request)
        {
            var predicate = PredicateExtensions.PredicateExtensions.Begin<ProjectTask>(true);
            if (!string.IsNullOrEmpty(request.UserName))
                predicate = predicate.And(p => p.User.Username.Contains(request.UserName));

            if (!string.IsNullOrEmpty(request.TaskTitle))
                predicate = predicate.And(p => p.Title.Contains(request.TaskTitle));

            if (!string.IsNullOrEmpty(request.ProjectTitle))
                predicate = predicate.And(p => p.Project.Title.Contains(request.ProjectTitle));

            return predicate;
        }

    }

}
