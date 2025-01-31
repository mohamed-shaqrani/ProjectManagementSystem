using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask.Commands;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.AddTask
{
    [Route("api/Task/")]
    public class AddTaskEndpoint : BaseEndpoint<AddTaskRequestViewModel, AddTaskResponseViewModel>
    {
        public AddTaskEndpoint(BaseEndpointParam<AddTaskRequestViewModel> param) : base(param)
        {
        }
        [Authorize(Policy = "ProjectAdmin")]
        [HttpPost]
        public async Task<EndpointResponse<AddTaskResponseViewModel>> CreateAsync([FromBody] AddTaskRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new AddTaskCommand(
                viewModel.Title, 
                viewModel.Description, 
                viewModel.Status) 
            { ProjectId = viewModel.ProjectId });

            return result.IsSuccess ? EndpointResponse<AddTaskResponseViewModel>
                                                    .Success(new AddTaskResponseViewModel
                                                    { Title = viewModel.Title }
                                                    , "Success")
                             : EndpointResponse<AddTaskResponseViewModel>.Failure(result.ErrorCode, result.Message);
        }
    }
}
