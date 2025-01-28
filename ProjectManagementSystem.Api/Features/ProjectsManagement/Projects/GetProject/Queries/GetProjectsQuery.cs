using MediatR;
using PredicateExtensions;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;
using System.Linq.Expressions;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject.Queries;

public record GetProjectsQuery(ProjectParam ProjectParam) : IRequest<RequestResult<PageList<ProjectResponseViewModel>>>;
public class GetProjectsQueryHandler : BaseRequestHandler<GetProjectsQuery, RequestResult<PageList<ProjectResponseViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectsQueryHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
    {
        _unitOfWork = unitOfWork;
    }


    public override async Task<RequestResult<PageList<ProjectResponseViewModel>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var predicate = BuildPredicate(request);


        var query = from p in _unitOfWork.GetRepository<Project>().GetAll(predicate)
                    join t in _unitOfWork.GetRepository<ProjectTask>().GetAll() on p.Id equals t.ProjectId into ptGroup
                    from t in ptGroup.DefaultIfEmpty()
                    join u in _unitOfWork.GetRepository<User>().GetAll() on t.UserID equals u.Id into tuGroup
                    from u in tuGroup.DefaultIfEmpty()
                    group new { p, t, u } by new { p.Id, p.Title } into g
                    select new ProjectResponseViewModel
                    {
                        ProjectId = g.Key.Id,
                        Title = g.Key.Title,
                        NumTasks = g.Count(x => x.t != null),
                        NumOfUsers = g.Where(x => x.t != null && x.u != null).Select(x => x.u.Id).Distinct().Count(),
                        DateCreated = g.First().p.CreatedAt
                    };


        var paginatedResult = await PageList<ProjectResponseViewModel>.CreateAsync(query, 1, 10);

        return RequestResult<PageList<ProjectResponseViewModel>>.Success(paginatedResult, "success");
    }


    private Expression<Func<Project, bool>> BuildPredicate(GetProjectsQuery request)
    {
        var predicate = PredicateExtensions.PredicateExtensions.Begin<Project>(true);
        if (!string.IsNullOrEmpty(request.ProjectParam.Title))

            predicate = predicate.And(p => p.Title.Contains(request.ProjectParam.Title));


        return predicate;
    }
}
