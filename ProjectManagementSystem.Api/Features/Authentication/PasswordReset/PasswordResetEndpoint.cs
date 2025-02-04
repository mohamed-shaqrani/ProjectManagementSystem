using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.PasswordReset;
using ProjectManagementSystem.Api.Features.Authentication.PasswordReset.PasswordReset.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.ResetPassword;

[Route("api/auth/reset-password")]
public class PasswordResetEndpoint : BaseEndpoint<PasswordResetViewModel, bool>
{
    public PasswordResetEndpoint(BaseEndpointParam<PasswordResetViewModel> param) : base(param)
    {
    }

    [HttpPost]
    public async Task<ActionResult<EndpointResponse<string>>> PasswordReset([FromBody] PasswordResetViewModel param)
    {
        var validationResult = ValidateRequest(param);
        if (!validationResult.IsSuccess)
            return BadRequest(EndpointResponse<string>.Failure(validationResult.ErrorCode, validationResult.Message));

        var command = new PasswordResetCommand(param.Email, param.NewPassword, param.OTP);
        var result = await _mediator.Send(command);

        return result.IsSuccess ? Ok( EndpointResponse<string>.Success(string.Empty, result.Message))
                                : Unauthorized( EndpointResponse<string>.Failure(result.ErrorCode, result.Message));
    }
}
