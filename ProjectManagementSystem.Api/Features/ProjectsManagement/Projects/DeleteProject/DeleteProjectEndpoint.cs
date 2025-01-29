using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.UpdateProject;

[Route("api/project/")]
public class DeleteProjectEndpoint : BaseEndpoint<DeleteProjectRequestViewModel, EndpointResponse<bool>>
{
    public DeleteProjectEndpoint(BaseEndpointParam<DeleteProjectRequestViewModel> param) : base(param)
    {

    }

    //  [Authorize(Roles ="User")]
    [HttpDelete]

    public async Task<EndpointResponse<bool>> Update([FromBody] DeleteProjectRequestViewModel param)
    {
        var query = new DeleteProjectCommand(param.Id);
        var res = await _mediator.Send(query);

        return res.IsSuccess ? EndpointResponse<bool>.Success(res.Data, res.Message)
                             : EndpointResponse<bool>.Failure(res.ErrorCode, res.Message);

    }
}
