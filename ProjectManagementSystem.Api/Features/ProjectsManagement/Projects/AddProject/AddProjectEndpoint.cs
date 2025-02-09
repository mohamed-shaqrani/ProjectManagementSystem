﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject.Commands;
using ProjectManagementSystem.Api.Filters;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.AddProject;

[Route("api/project/")]
public class AddProjectEndpoint : BaseEndpoint<AddProjectRequestViewModel, EndpointResponse<bool>>
{
    public AddProjectEndpoint(BaseEndpointParam<AddProjectRequestViewModel> param) : base(param)
    {

    }

    [HttpPost]
    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { Feature.AddProject })]

    public async Task<EndpointResponse<bool>> AddProject([FromBody] AddProjectRequestViewModel param)
    {
        var query = new AddProjectCommand(param.Title);
        var res = await _mediator.Send(query);

        return res.IsSuccess ? EndpointResponse<bool>.Success(default, "Success")
                             : EndpointResponse<bool>.Failure(res.ErrorCode, res.Message);

    }
}
