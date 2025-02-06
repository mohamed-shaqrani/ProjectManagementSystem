using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.UpdateStatus.UpdateTask.Commands;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.UpdateStatus.UpdateTask
{
    [Route("api/task-status")]

    public class UpdateTaskStatusEndpoint : BaseEndpoint<UpdateTaskStatusRequestViewModel, EndpointResponse<bool>>
    {
        public UpdateTaskStatusEndpoint(BaseEndpointParam<UpdateTaskStatusRequestViewModel> param) : base(param)
        {
        }

        [HttpPut]
        public async Task<ActionResult<EndpointResponse<bool>>> Update([FromBody] UpdateTaskStatusRequestViewModel viewModel)
        {
            var validationResult = ValidateRequest(viewModel);
            if (!validationResult.IsSuccess)
            {
                return BadRequest(validationResult.Message);
            }
            var result = await _mediator.Send(new UpdateTaskStatusCommand(viewModel.TaskID, viewModel.Status));
            return result.IsSuccess ? Ok(EndpointResponse<bool>.Success(true, "Success"))
                                    : StatusCode(500, EndpointResponse<bool>.Failure(result.ErrorCode, result.Message));
        }

    }
}
