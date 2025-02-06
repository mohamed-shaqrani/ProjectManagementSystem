using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject.Queries;
using ProjectManagementSystem.Api.Filters;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;

[Route("api/project/")]
public class GetProjectsEndpoint : BaseEndpoint<AddProjectRequestViewModel, AddProjectResponseViewModel>
{
    public GetProjectsEndpoint(BaseEndpointParam<AddProjectRequestViewModel> param) : base(param)
    {

    }
    [HttpGet]
    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { Feature.ViewProject })]


    public async Task<EndpointResponse<PageList<ProjectResponseViewModel>>> GetAll([FromQuery] ProjectParam projectParam)
    {
        var query = new GetProjectsQuery(projectParam);
        var res = await _mediator.Send(query);
        Response.AddPaginationHeader(res.Data);

        return res.IsSuccess ? EndpointResponse<PageList<ProjectResponseViewModel>>.Success(res.Data, "Success")
                             : EndpointResponse<PageList<ProjectResponseViewModel>>.Failure(res.ErrorCode, res.Message);

    }
}
