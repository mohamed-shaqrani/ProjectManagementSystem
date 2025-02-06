using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask.Commands;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask
{
    [Route("api/task/")]
    public class AddTaskEndpoint : BaseEndpoint<AddTaskRequestViewModel, AddTaskResponseViewModel>
    {
        public AddTaskEndpoint(BaseEndpointParam<AddTaskRequestViewModel> param) : base(param)
        {
        }

        [HttpPost]
        public async Task<ActionResult<EndpointResponse<AddTaskResponseViewModel>>> CreateAsync([FromBody] AddTaskRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new AddTaskCommand(viewModel.Title, viewModel.Description, viewModel.Status, viewModel.UserID, viewModel.ProjectID));

            return result.IsSuccess ? Ok(EndpointResponse<AddTaskResponseViewModel>.Success(new AddTaskResponseViewModel { Title = viewModel.Title }, "Success"))
                                    : StatusCode(500, EndpointResponse<AddTaskResponseViewModel>.Failure(result.ErrorCode, result.Message));
        }
    }
}
