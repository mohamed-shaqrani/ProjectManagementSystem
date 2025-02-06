using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask.Queries;
using ProjectManagementSystem.Api.Features.UserManagement.GetUsers.Queries;
using ProjectManagementSystem.Api.Response.Endpint;
using System.Security.Claims;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask
{
    [Route("api/user-tasks/")]
    public class GetUserTasksGroupedByStatusEndpoint : BaseEndpoint<TaskParam, IEnumerable<TaskDTO>>
    {
        public GetUserTasksGroupedByStatusEndpoint(BaseEndpointParam<TaskParam> param) : base(param)
        {
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>> GetTasks()
        {
            var userEmail = User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.Email)!.Value;
            var userId = await _mediator.Send(new GetUserIdByEmailQuery(userEmail));
            var result = await _mediator.Send(new GetUserTasksGroupedByStatusQuery(userId.Data));


            return result.IsSuccess ? Ok(EndpointResponse<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>.Success(result.Data, "Success"))
                                : NotFound(EndpointResponse<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>.Failure(result.ErrorCode, result.Message));


        }
    }
}
