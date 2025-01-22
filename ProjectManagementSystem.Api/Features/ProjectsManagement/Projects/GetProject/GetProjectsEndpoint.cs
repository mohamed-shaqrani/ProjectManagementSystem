using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;

[Route("api/project/")]
public class GetProjectsEndpoint : BaseEndpoint<AddProjectRequestViewModel, AddProjectResponseViewModel>
{
    public GetProjectsEndpoint(BaseEndpointParam<AddProjectRequestViewModel> param) : base(param)
    {

    }
    [HttpGet]
    public async Task<EndpointResponse<IEnumerable<ProjectResponseViewModel>>> GetAll()
    {
        var query = new GetProjectsQuery();
        var res = await _mediator.Send(query);

        return res.IsSuccess ? EndpointResponse<IEnumerable<ProjectResponseViewModel>>.Success(res.Data, "Success")
                             : EndpointResponse<IEnumerable<ProjectResponseViewModel>>.Failure(res.ErrorCode, res.Message);

    }
}
