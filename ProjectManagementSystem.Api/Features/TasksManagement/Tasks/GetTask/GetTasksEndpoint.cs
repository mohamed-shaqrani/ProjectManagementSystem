using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask.Queries;
using ProjectManagementSystem.Api.Filters;
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
        [Authorize]
        [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { Feature.ViewTask })]

        public async Task<ActionResult<EndpointResponse<TaskDTO>>> GetTasks([FromQuery] TaskParam taskParam)
        {
            var result = await _mediator.Send(new GetTasksQuery(taskParam));

            Response.AddPaginationHeader(result.Data);

            return result.IsSuccess ? Ok(EndpointResponse<PageList<TaskDTO>>.Success(result.Data, "Success"))
                                : NotFound(EndpointResponse<PageList<TaskDTO>>.Failure(result.ErrorCode, result.Message));


        }
    }
}
