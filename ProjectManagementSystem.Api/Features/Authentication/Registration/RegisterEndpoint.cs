using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Authentication.Registration.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration
{
    [Route("api/register")]
    public class RegisterEndpoint : BaseEndpoint<RegisterViewModel, EndpointResponse<bool>>
    {
        public RegisterEndpoint(BaseEndpointParam<RegisterViewModel> param) : base(param)
        {
        }

        [HttpPost]

        public async Task<EndpointResponse<AuthModel>> Register([FromForm] RegisterViewModel model)
        {
            var register = new RegisterCommand(model.Username, model.Email, model.Password, model.imageFile);

            var res = await _mediator.Send(register);

            if (res.IsSuccess)
            {
                return EndpointResponse<AuthModel>.Success(res.Data, "Register Successfully");
            }

            return EndpointResponse<AuthModel>.Failure(res.ErrorCode, "Register Unsuccessfully");
        }
    }
}
