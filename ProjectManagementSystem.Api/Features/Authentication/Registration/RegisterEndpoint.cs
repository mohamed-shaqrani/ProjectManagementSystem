using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Authentication.Registration.Command;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration
{
    [Route("api/auth/register")]
    public class RegisterEndpoint : BaseEndpoint<RegisterViewModel, EndpointResponse<string>>
    {
        public RegisterEndpoint(BaseEndpointParam<RegisterViewModel> param) : base(param)
        {
        }

        [HttpPost]
        public async Task<EndpointResponse<string>> Register([FromForm] RegisterViewModel model)
        {
            var validationResult = ValidateRequest(model);
            if (!validationResult.IsSuccess)
            {
                return EndpointResponse<string>.Failure(validationResult.ErrorCode, validationResult.Message);
            }
            var register = new RegisterCommand(model.Username, model.Email, model.Password, model.phone, model.imageFile);

            var res = await _mediator.Send(register);

            if (res.IsSuccess)
            {
                return EndpointResponse<string>.Success(res.Data, "Register Successfully");
            }

            return EndpointResponse<string>.Failure(res.ErrorCode, "Register Unsuccessfully");
        }
    }
}
