using AutoMapper.QueryableExtensions;
using MediatR;
using PredicateExtensions;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.MappingProfiles;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;
using System.Linq.Expressions;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;

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

        var result = _unitOfWork.GetRepository<Project>()
                                                .GetAll(predicate)
                                                 .GroupBy(p => new ProjectGroupKey
                                                 {
                                                     Title = p.Title,
                                                     Status = p.Status,
                                                     CreatedAt = p.CreatedAt
                                                 })
                                               .ProjectTo<ProjectResponseViewModel>();



        var paginatedResult = await PageList<ProjectResponseViewModel>.CreateAsync(result, request.ProjectParam.PageNumber, request.ProjectParam.PageSize);

        return RequestResult<PageList<ProjectResponseViewModel>>.Success(paginatedResult, "success");
    }


    private Expression<Func<Project, bool>> BuildPredicate(GetProjectsQuery request)
    {
        var predicate = PredicateExtensions.PredicateExtensions.Begin<Project>(true);
        if (!string.IsNullOrEmpty(request.ProjectParam.Title))

            predicate.And(p => p.Title.Contains(request.ProjectParam.Title));


        return predicate;
    }
}
