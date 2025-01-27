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

        var result2 = from p in _unitOfWork.GetRepository<Project>().GetAll(predicate)
                      join t in _unitOfWork.GetRepository<ProjectTask>().GetAll() on p.Id equals t.ProjectId
                      group t by new { p.Title, p.Status, p.CreatedAt } into g
                      select new ProjectResponseViewModel
                      {
                          Title = g.Key.Title,
                          Status = g.Key.Status,
                          NumTasks = g.Count(),
                          NumOfUsers = g.Select(t => t.UserID).Distinct().Count(),
                          DateCreated = g.Key.CreatedAt
                      };


        var paginatedResult = await PageList<ProjectResponseViewModel>.CreateAsync(result2, request.ProjectParam.PageNumber, request.ProjectParam.PageSize);

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
