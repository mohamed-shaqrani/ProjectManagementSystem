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

        public async Task<EndpointResponse<AuthModel>> Register([FromBody] ConfirmAccountViewModel model)
        {
            var validationResult = ValidateRequest(model);
            if (!validationResult.IsSuccess)
                return EndpointResponse<AuthModel>.Failure(validationResult.ErrorCode, validationResult.Message);

            var register = new ConfirmAccountCommand(model.code);

            var res = await _mediator.Send(register);

            if (res.IsSuccess)
            {
                return EndpointResponse<AuthModel>.Success(res.Data, "Register Successfully");
            }

            return EndpointResponse<AuthModel>.Failure(res.ErrorCode, "Register Unsuccessfully");
        }
    }
}
