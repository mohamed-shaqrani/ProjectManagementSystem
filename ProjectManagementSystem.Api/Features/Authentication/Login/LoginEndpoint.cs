using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.Login.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.Login
{
    [Route("api/auth/login")]
    public class LoginEndpoint : BaseEndpoint<LoginViewModel, LoginViewModel>
    {
        public LoginEndpoint(BaseEndpointParam<LoginViewModel> param) : base(param)
        {

        }


        [HttpPost]

        public async Task<EndpointResponse<AuthModel>> Login([FromBody] LoginViewModel model)
        {
            var validationResult = ValidateRequest(model);
            if (!validationResult.IsSuccess)
                return EndpointResponse<AuthModel>.Failure(validationResult.ErrorCode, validationResult.Message);

            var logincommand = new LoginCommand(model.Email, model.Password);

            var res = await _mediator.Send(logincommand);

            if (res.IsSuccess)
            {
                return EndpointResponse<AuthModel>.Success(res.Data, "Login Successfully");
            }

            return EndpointResponse<AuthModel>.Failure(res.ErrorCode.Value, res.Message);
        }
    }
}
