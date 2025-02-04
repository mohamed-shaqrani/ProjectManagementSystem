using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask.Queries;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask
{
    [Route("api/Task/")]
    public class GetTasksEndpoint : BaseEndpoint<TaskRequestViewModel, IEnumerable<TaskResponseViewModel>>
    {
        public GetTasksEndpoint(BaseEndpointParam<TaskRequestViewModel> param) : base(param)
        {
        }

        [HttpGet]
        public async Task<EndpointResponse<IEnumerable<TaskResponseViewModel>>> GetTasks()
        {
            var result = await _mediator.Send(new GetTasksQuery());


            var responseViewModels = result.Data.Select(x=>new TaskResponseViewModel
            {
                Title = x.Title,
                Status = x.Status,
                UserName = x.UserName,
                ProjectName = x.ProjectName,
                DateCreated = x.DateCreated,
            });

            return result.IsSuccess ?
                EndpointResponse<IEnumerable<TaskResponseViewModel>>
                                .Success(responseViewModels, "Success")
                             : EndpointResponse<IEnumerable<TaskResponseViewModel>>
                                .Failure(result.ErrorCode, result.Message);


        }
    }
}
