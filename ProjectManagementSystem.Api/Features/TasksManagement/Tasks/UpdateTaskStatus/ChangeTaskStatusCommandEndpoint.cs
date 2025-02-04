using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTaskStatus.command;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTaskStatus
{
    public class ChangeTaskStatusCommandEndpoint : BaseEndpoint<ChangeTaskStatusRequestViewModel,ChangeTaskStatusRequestViewModel>
    {
        public ChangeTaskStatusCommandEndpoint(BaseEndpointParam<ChangeTaskStatusRequestViewModel> param) : base(param) 
        {
            
        }

        [Authorize(Roles = "User")]
        [HttpPut]
        public async Task<EndpointResponse<bool>> update(ChangeTaskStatusRequestViewModel viewModel) 
        {
            var command = new ChangeTaskStatusCommand(viewModel.taskid,viewModel.Status);

            var result = await _mediator.Send(command);

            return result.IsSuccess ? EndpointResponse<bool>.Success(true, "Task updated successfully") : 
                EndpointResponse<bool>.Failure(Api.Response.ErrorCode.NotFound, "Task is not found");

                 
        }
    }
}
