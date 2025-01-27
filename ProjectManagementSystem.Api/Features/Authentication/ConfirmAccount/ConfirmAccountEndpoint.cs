using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.ConfirmAccount.command;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.ConfirmAccount
{
    [Route("api/confirm")]
    public class ConfirmAccountEndpoint : BaseEndpoint<ConfirmAccountViewModel, EndpointResponse<string>>
    {
        public ConfirmAccountEndpoint(BaseEndpointParam<ConfirmAccountViewModel> param) : base(param)
        {
        }

        [HttpPost]

        public async Task<EndpointResponse<AuthModel>> Register([FromForm] string email, int code)
        {
            var register = new ConfirmAccountCommand(email, code);

            var res = await _mediator.Send(register);

            if (res.IsSuccess)
            {
                return EndpointResponse<AuthModel>.Success(res.Data, "Register Successfully");
            }

            return EndpointResponse<AuthModel>.Failure(res.ErrorCode, "Register Unsuccessfully");
        }
    }
}
