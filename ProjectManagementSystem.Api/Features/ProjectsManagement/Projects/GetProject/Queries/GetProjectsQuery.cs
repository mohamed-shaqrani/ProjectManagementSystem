using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;

public record GetProjectsQuery() : IRequest<RequestResult<IEnumerable<ProjectResponseViewModel>>>;
public class GetProjectsQueryHandler : BaseRequestHandler<GetProjectsQuery, RequestResult<IEnumerable<ProjectResponseViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectsQueryHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
    {
        _unitOfWork = unitOfWork;
    }


    public override async Task<RequestResult<IEnumerable<ProjectResponseViewModel>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.GetRepository<Project>()
            .GetAll()
            .GroupBy(p => new { p.Title, p.Status, p.CreatedAt })
            .Select(p => new ProjectResponseViewModel
            {
                Title = p.Key.Title,
                Status = p.Key.Status,
                NumTasks = p.Count(),
                DateCreated = p.Key.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return RequestResult<IEnumerable<ProjectResponseViewModel>>.Success(result, "success");
    }


}
