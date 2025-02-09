﻿using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.Registration.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration
{
    [Route("api/auth/register")]
    public class RegisterEndpoint : BaseEndpoint<RegisterViewModel, EndpointResponse<string>>
    {
        public RegisterEndpoint(BaseEndpointParam<RegisterViewModel> param) : base(param)
        {
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<string>>> Register([FromForm] RegisterViewModel model)
        {
            var validationResult = ValidateRequest(model);
            if (!validationResult.IsSuccess)
            {
                return BadRequest(validationResult.Message);
            }
            var register = new RegisterCommand(model.Username, model.Email, model.Password, model.Phone, model.imageFile);

            var res = await _mediator.Send(register);

            if (res.IsSuccess)
            {
                return Ok( EndpointResponse<string>.Success(res.Data, res.Message));
            }
            //
            return StatusCode(500, EndpointResponse<string>.Failure(ErrorCode.InternalServerError, "An unexpected error occurred."));
        }
    }
}
