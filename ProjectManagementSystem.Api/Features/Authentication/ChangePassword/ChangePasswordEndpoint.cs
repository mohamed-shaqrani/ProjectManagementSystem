using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.ChangePassword.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.ChangePassword;

[Authorize]
[Route("api/auth/change-password/")]
public class ChangePasswordEndpoint : BaseEndpoint<ChangePasswordViewModel, string>
{
    public ChangePasswordEndpoint(BaseEndpointParam<ChangePasswordViewModel> param) : base(param)
    {
    }

    [HttpPost]
    public async Task<EndpointResponse<string>> ChangePassword([FromBody] ChangePasswordViewModel param)
    {
        var validationResult = ValidateRequest(param);
        if (!validationResult.IsSuccess)
            return EndpointResponse<string>.Failure(validationResult.ErrorCode, validationResult.Message);

        var result = await _mediator.Send(new ChangePasswordCommand(param.UserId, param.OldPassword, param.NewPassword));

        return result.IsSuccess ? EndpointResponse<string>.Success(string.Empty, "Success changed password")
                         : EndpointResponse<string>.Failure(result.ErrorCode, result.Message);
    }

}