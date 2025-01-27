using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.PasswordReset.PasswordReset.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.ResetPassword;

[Route("api/auth/resetpassword/")]
public class PasswordResetEndpoint : BaseEndpoint<PasswordResetViewModel, bool>
{
    public PasswordResetEndpoint(BaseEndpointParam<PasswordResetViewModel> param) : base(param)
    {
    }

    [HttpPost]
    public async Task<EndpointResponse<string>> PasswordReset([FromBody] PasswordResetViewModel param)
    {
        var command = new PasswordResetCommand(param.Email, param.NewPassword, param.OTP);
        var result = await _mediator.Send(command);

        return result.IsSuccess ? EndpointResponse<string>.Success(string.Empty, "Email Sent with OTP verification code")
                         : EndpointResponse<string>.Failure(result.ErrorCode, result.Message);
    }
}
