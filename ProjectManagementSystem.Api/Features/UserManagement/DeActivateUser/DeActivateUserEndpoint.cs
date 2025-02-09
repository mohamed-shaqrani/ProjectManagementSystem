﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.UserManagement.DeActivateUser.Commands;
using ProjectManagementSystem.Api.Filters;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.UserManagement.DeActivateUser;

[Route("api/de-activate-user/")]
public class DeActivateUserEndpoint : BaseEndpoint<DeActivateUserRequestViewModel, EndpointResponse<bool>>
{
    public DeActivateUserEndpoint(BaseEndpointParam<DeActivateUserRequestViewModel> param) : base(param)
    {

    }

    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { Feature.DeActivateUser })]
    [HttpPut]
    public async Task<ActionResult<EndpointResponse<bool>>> Update([FromQuery] DeActivateUserRequestViewModel param)
    {
        var validationResult = ValidateRequest(param);
        if (!validationResult.IsSuccess)
            return BadRequest(EndpointResponse<bool>.Failure(validationResult.ErrorCode, validationResult.Message));

        var query = new DeActivateUserCommand(param.UserId, param.DeActive);

        var res = await _mediator.Send(query);
        if (res.ErrorCode == Api.Response.ErrorCode.DataBaseError)
            return StatusCode(500, EndpointResponse<bool>.Failure(res.ErrorCode, res.Message));

        return res.IsSuccess ? Ok(EndpointResponse<bool>.Success(default, res.Message))
                             : StatusCode(500, EndpointResponse<bool>.Failure(res.ErrorCode, res.Message));

    }
}
