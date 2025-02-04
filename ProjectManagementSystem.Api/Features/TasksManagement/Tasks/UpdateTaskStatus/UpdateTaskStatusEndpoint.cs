using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTaskStatus.Commands;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTaskStatus
{
    [Route("api/Task/Change-status")]
    public class UpdateTaskStatusEndpoint : BaseEndpoint<UpdateTaskStatusViewModel, EndpointResponse<bool>>
    {
        public UpdateTaskStatusEndpoint(BaseEndpointParam<UpdateTaskStatusViewModel> param) : base(param)
        {
        }

        [HttpPut]
        public async Task<EndpointResponse<bool>> Update([FromBody] UpdateTaskStatusViewModel param)
        {
            var result = await _mediator.Send(new UpdateTaskStatusCommand(param.TaskID, param.NewStatus));
            return result.IsSuccess ? EndpointResponse<bool>.Success(true , "Success")
                                    : EndpointResponse<bool>.Failure(result.ErrorCode, result.Message);
        }

    }
}
