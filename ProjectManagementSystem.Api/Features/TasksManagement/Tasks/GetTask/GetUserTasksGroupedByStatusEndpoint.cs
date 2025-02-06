using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask.Queries;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask
{
    [Route("api/user-tasks/")]
    public class GetUserTasksGroupedByStatusEndpoint : BaseEndpoint<TaskParam, IEnumerable<TaskDTO>>
    {
        public GetUserTasksGroupedByStatusEndpoint(BaseEndpointParam<TaskParam> param) : base(param)
        {
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>> GetTasks(int userID)
        {
            var result = await _mediator.Send(new GetUserTasksGroupedByStatusQuery(userID));


            return result.IsSuccess ? Ok(EndpointResponse<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>.Success(result.Data, "Success"))
                                : NotFound(EndpointResponse<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>.Failure(result.ErrorCode, result.Message));


        }
    }
}
