using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;
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
    public async Task<EndpointResponse<IEnumerable<ProjectResponseViewModel>>> GetAll([FromQuery] ProjectParam projectParam)
    {
        var query = new GetProjectsQuery(projectParam);
        var res = await _mediator.Send(query);
        Response.AddPaginationHeader(res.Data);

        return res.IsSuccess ? EndpointResponse<IEnumerable<ProjectResponseViewModel>>.Success(res.Data, "Success")
                             : EndpointResponse<IEnumerable<ProjectResponseViewModel>>.Failure(res.ErrorCode, res.Message);

    }
}
