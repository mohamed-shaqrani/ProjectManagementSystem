using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.ForgetPassword.Commands;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.ForgetPassword
{
    [Route("api/auth/forget/")]
    public class ForgetPasswordEndpoint : BaseEndpoint<ForgetPassRequestViewModel, string>
    {
        public ForgetPasswordEndpoint(BaseEndpointParam<ForgetPassRequestViewModel> param) : base(param)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<string>> ForgetPassword([FromBody] ForgetPassRequestViewModel param)
        {
            var result = await _mediator.Send(new ForgetPasswordCommand(param.Email));

            return result.IsSuccess ? EndpointResponse<string>.Success(string.Empty, "Email Sent with OTP verification code")
                             : EndpointResponse<string>.Failure(result.ErrorCode, result.Message);
        }

    }
}
