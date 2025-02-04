using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask.Queries;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask
{
    [Route("api/task/")]
    public class GetTasksEndpoint : BaseEndpoint<TaskParam, IEnumerable<TaskDTO>>
    {
        public GetTasksEndpoint(BaseEndpointParam<TaskParam> param) : base(param)
        {
        }

        [HttpGet]
        public async Task<ActionResult<EndpointResponse<TaskDTO>>> GetTasks(TaskParam taskParam)
        {
            var result = await _mediator.Send(new GetTasksQuery(taskParam));

            return result.IsSuccess ? Ok(EndpointResponse<PageList<TaskDTO>>.Success(result.Data, "Success"))
                                : StatusCode(500, EndpointResponse<PageList<TaskDTO>>.Failure(result.ErrorCode, result.Message));


        }
    }
}
