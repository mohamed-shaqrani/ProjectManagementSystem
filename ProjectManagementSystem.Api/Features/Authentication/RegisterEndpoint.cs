﻿using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.Commands;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication
{
    [Route("api/register")]
    public class RegisterEndpoint : BaseEndpoint<RegisterViewModel, RegisterViewModel>
    {
        public RegisterEndpoint(BaseEndpointParam<RegisterViewModel> param): base(param) 
        {
        }

        [HttpPost]

        public async Task<EndpointResponse<bool>> Register([FromBody] RegisterViewModel model)
        {
            var register = new RegisterCommand(model.Username, model.Email,model.Password);

            var res = await _mediator.Send(register);

            if (res.IsSuccess)
            {
                return EndpointResponse<bool>.Success(true, "Login Successfully");
            }

            return EndpointResponse<bool>.Failure(res.ErrorCode.Value, "Register Unsuccessfully");
        }
    }
}
