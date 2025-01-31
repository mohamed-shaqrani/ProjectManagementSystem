using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask.Commands;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.DeleteTask
{
    [Route("api/Task/")]
    public class DeleteTaskEndpoint : BaseEndpoint<DeleteTaskRequestViewModel, bool>
    {
        public DeleteTaskEndpoint(BaseEndpointParam<DeleteTaskRequestViewModel> param) : base(param)
        {
        }

        [HttpDelete("{TaskID}")]
        public async Task<EndpointResponse<bool>> Delete(int TaskID)
        {
           var result = await _mediator.Send(new DeleteTaskCommand(TaskID));

            return result.IsSuccess ? EndpointResponse<bool>.Success(true , "Deleted Successfully")
                                    : EndpointResponse<bool>.Failure(result.ErrorCode, result.Message);
        }

    }
}
