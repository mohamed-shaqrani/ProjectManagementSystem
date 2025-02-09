﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;
using ProjectManagementSystem.Api.Filters;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.DeleteProject;

[Route("api/project/")]
public class DeleteProjectEndpoint : BaseEndpoint<DeleteProjectRequestViewModel, EndpointResponse<bool>>
{
    public DeleteProjectEndpoint(BaseEndpointParam<DeleteProjectRequestViewModel> param) : base(param)
    {

    }

    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { Feature.DeleteProject })]

    [HttpDelete]

    public async Task<ActionResult<EndpointResponse<bool>>> Delete([FromBody] DeleteProjectRequestViewModel param)
    {
        var validationResult = ValidateRequest(param);
        var query = new DeleteProjectCommand(param.Id);
        var res = await _mediator.Send(query);
        if (!res.IsSuccess && res.ErrorCode == Api.Response.ErrorCode.ProjectNotExist)

            return NotFound();

        return res.IsSuccess ? Ok(EndpointResponse<bool>.Success(res.Data, res.Message))
                             : BadRequest(EndpointResponse<bool>.Failure(res.ErrorCode, res.Message));

    }
}
