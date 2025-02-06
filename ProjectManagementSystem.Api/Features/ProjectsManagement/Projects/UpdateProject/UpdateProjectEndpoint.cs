using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.UpdateProject.Commands;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.UpdateProject;

[Route("api/project/")]
public class UpdateProjectEndpoint : BaseEndpoint<UpdateProjectRequestViewModel, EndpointResponse<bool>>
{
    public UpdateProjectEndpoint(BaseEndpointParam<UpdateProjectRequestViewModel> param) : base(param)
    {

    }

    //  [Authorize(Roles ="User")]
    [HttpPut]

    public async Task<ActionResult<EndpointResponse<bool>>> Update([FromBody] UpdateProjectRequestViewModel param)
    {
        var validationResult = ValidateRequest(param);
        if (!validationResult.IsSuccess)
        {
            return BadRequest(validationResult.Message);
        }
        var query = new UpdateProjectCommand(param.Title, 1);
        var res = await _mediator.Send(query);

        return res.IsSuccess ? Ok(EndpointResponse<bool>.Success(default, "Success"))
                             : StatusCode(500, EndpointResponse<bool>.Failure(res.ErrorCode, res.Message));

    }
}
