using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.UpdateStatus.UpdateTask.Commands;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.Endpint;
using System.Security.Claims;

namespace ProjectManagementSystem.Api.Features.TasksManagement.UpdateStatus.UpdateTask
{
    [Route("api/task-status")]

    public class UpdateTaskStatusEndpoint : BaseEndpoint<UpdateTaskStatusRequestViewModel, EndpointResponse<bool>>
    {
        public UpdateTaskStatusEndpoint(BaseEndpointParam<UpdateTaskStatusRequestViewModel> param) : base(param)
        {
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<EndpointResponse<bool>>> Update([FromBody] UpdateTaskStatusRequestViewModel viewModel)
        {
            var validationResult = ValidateRequest(viewModel);
            if (!validationResult.IsSuccess)
            {
                return BadRequest(validationResult.Message);
            }
            var role = User.FindFirst(ClaimTypes.Role).Value;
            bool isAdmin = false;
            if (role == "Admin")
            {
                isAdmin = true;
            }
            var email = User.FindFirst(ClaimTypes.Email).Value;

            var result = await _mediator.Send(new UpdateTaskStatusCommand(viewModel.TaskID, viewModel.Status, isAdmin, email));
            if (!result.IsSuccess && result.ErrorCode == ErrorCode.TaskDoesNotBelongToUser)
            {
                return Unauthorized(EndpointResponse<bool>.Failure(result.ErrorCode, result.Message));
            }
            return result.IsSuccess ? Ok(EndpointResponse<bool>.Success(true, "Success"))
                                    : StatusCode(500, EndpointResponse<bool>.Failure(result.ErrorCode, result.Message));
        }

    }
}
