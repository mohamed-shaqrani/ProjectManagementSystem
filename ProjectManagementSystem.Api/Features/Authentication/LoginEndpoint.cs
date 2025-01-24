﻿using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.Commands;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication
{
    [Route("api/Login/")]
    public class LoginEndpoint : BaseEndpoint<LoginViewModel, LoginViewModel>
    {
        public LoginEndpoint(BaseEndpointParam<LoginViewModel> param) : base(param) 
        {

        }


        [HttpPost]

        public async Task<EndpointResponse<AuthanticationModel>> LogiIn ([FromBody]LoginViewModel model) 
        {
            var logincommand = new LoginCommand( model.Email, model.Password);

            var res = await _mediator.Send(logincommand);

            if (res.IsSuccess) 
            {
                return EndpointResponse<AuthanticationModel>.Success(res.Data, "Login Successfully");
            }

            return EndpointResponse<AuthanticationModel>.Failure(res.ErrorCode.Value, "Login Unsuccessfully");
        }
    }
}
