using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask.Commands;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask.Queries;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask
{
    [Route("api/task/")]
    public class DeleteTaskEndpoint : BaseEndpoint<DeleteTaskRequestViewModel, bool>
    {
        public DeleteTaskEndpoint(BaseEndpointParam<DeleteTaskRequestViewModel> param) : base(param)
        {
        }

        [HttpDelete("{vm}")]
        public async Task<ActionResult<EndpointResponse<bool>>> Delete(int taskId)
        {
            if (taskId < 1)
            {
                return BadRequest("provide valid id");
            }
            var doesTaskExist = await _mediator.Send(new IsTaskExistQuery(taskId));

            if (!doesTaskExist.IsSuccess)
                return Unauthorized(EndpointResponse<bool>.Failure(doesTaskExist.ErrorCode, doesTaskExist.Message));

            var result = await _mediator.Send(new DeleteTaskCommand(taskId));

            return result.IsSuccess ? EndpointResponse<bool>.Success(true, "Deleted Successfully")
                                    : StatusCode(500, EndpointResponse<bool>.Failure(result.ErrorCode, result.Message));
        }

    }
}
