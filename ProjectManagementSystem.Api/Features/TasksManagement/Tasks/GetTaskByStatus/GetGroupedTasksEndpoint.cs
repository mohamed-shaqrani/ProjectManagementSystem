using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTaskByStatus.Queries;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTaskByStatus
{
    [Route("api/Task/Grouped-tasks")]
    public class GetGroupedTasksEndpoint : BaseEndpoint<GroupedTasksRequestViewModel, IEnumerable<GroupedTaskResponseViewModel>>
    {
        public GetGroupedTasksEndpoint(BaseEndpointParam<GroupedTasksRequestViewModel> param) : base(param)
        {
        }

        [HttpGet]
        public async Task<EndpointResponse<IEnumerable<GroupedTaskResponseViewModel>>> GetTasks()
        {
            var result = await _mediator.Send(new GetTasksByStatusQuery());


            var responseViewModels = result.Data.Select(x=>new GroupedTaskResponseViewModel
            {
                Status = x.Status,
                Tasks = x.Tasks.Select(t=>new TaskPreviewViewModel
                {
                    Id = t.Id,
                    Title = t.Title
                }).ToList()
            });

            return result.IsSuccess ?
                EndpointResponse<IEnumerable<GroupedTaskResponseViewModel>>
                                .Success(responseViewModels, "Success")
                             : EndpointResponse<IEnumerable<GroupedTaskResponseViewModel>>
                                .Failure(result.ErrorCode, result.Message);


        }
    }
}
