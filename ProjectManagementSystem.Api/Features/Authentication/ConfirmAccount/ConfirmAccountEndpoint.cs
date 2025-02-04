using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.ConfirmAccount.command;
using ProjectManagementSystem.Api.Features.Authentication.Login;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.ConfirmAccount
{
    [Route("api/auth/confirm-account")]
    public class ConfirmAccountEndpoint : BaseEndpoint<ConfirmAccountViewModel, EndpointResponse<string>>
    {
        public ConfirmAccountEndpoint(BaseEndpointParam<ConfirmAccountViewModel> param) : base(param)
        {
        }

        [HttpPost]

        public async Task<ActionResult<EndpointResponse<AuthModel>>> Register([FromBody] ConfirmAccountViewModel model)
        {
            var validationResult = ValidateRequest(model);
            if (!validationResult.IsSuccess)
                return BadRequest(EndpointResponse<AuthModel>.Failure(validationResult.ErrorCode, validationResult.Message));

            var register = new ConfirmAccountCommand(model.code);

            var res = await _mediator.Send(register);

            return res.IsSuccess ? Ok(EndpointResponse<AuthModel>.Success(res.Data, res.Message))

                                 : Unauthorized(EndpointResponse<AuthModel>.Failure(res.ErrorCode, res.Message));
        }
    }
}
