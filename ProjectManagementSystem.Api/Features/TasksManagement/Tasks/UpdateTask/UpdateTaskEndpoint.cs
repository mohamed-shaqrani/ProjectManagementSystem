using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTask.Commands;
using ProjectManagementSystem.Api.Filters;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.UpdateTask
{
    [Route("api/task")]

    public class UpdateTaskEndpoint : BaseEndpoint<UpdateTaskRequestViewModel, EndpointResponse<bool>>
    {
        public UpdateTaskEndpoint(BaseEndpointParam<UpdateTaskRequestViewModel> param) : base(param)
        {
        }

        [HttpPut]
        [Authorize]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { Feature.UpdateTask })]

        public async Task<ActionResult<EndpointResponse<bool>>> Update([FromBody] UpdateTaskRequestViewModel viewModel)
        {
            var result = await _mediator.Send(new UpdateTaskCommand(viewModel.TaskID, viewModel.Title, viewModel.Description, viewModel.Status, viewModel.UserID, viewModel.ProjectID));
            return result.IsSuccess ? Ok(EndpointResponse<bool>.Success(true, "Success"))
                                    : StatusCode(500, EndpointResponse<bool>.Failure(result.ErrorCode, result.Message));
        }

    }
}
