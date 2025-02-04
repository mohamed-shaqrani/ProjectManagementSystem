using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTask.Commands;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTask
{
    [Route("api/Task")]

    public class UpdateTaskEndpoint : BaseEndpoint<UpdateTaskRequestViewModel , EndpointResponse<bool>>
    {
        public UpdateTaskEndpoint(BaseEndpointParam<UpdateTaskRequestViewModel> param) : base(param)
        {
        }

        [HttpPut]
        public async Task<EndpointResponse<bool>> Update([FromBody] UpdateTaskRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new UpdateTaskCommand(viewModel.TaskID, viewModel.Title, viewModel.Description, viewModel.Status, viewModel.UserID, viewModel.ProjectID));
            return result.IsSuccess ? EndpointResponse<bool>.Success(true, "Success")
                                    : EndpointResponse<bool>.Failure(result.ErrorCode, result.Message);
        }

    }
}
