using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;


namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTaskByStatus.Queries
{
    public record GetTasksByStatusQuery():IRequest<RequestResult<IEnumerable<GroupedTasksByStatus>>>;

    public class GetTasksByStatusQueryHandler : BaseRequestHandler<GetTasksByStatusQuery, RequestResult<IEnumerable<GroupedTasksByStatus>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTasksByStatusQueryHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
        {
            _unitOfWork = unitOfWork;
        }
        public override async Task<RequestResult<IEnumerable<GroupedTasksByStatus>>> Handle(GetTasksByStatusQuery request, CancellationToken cancellationToken)
        {
            var response = await ValidateRequest(request);
            if (!response.IsSuccess)
            {
                return response;
            }

            var groupedTasks = _unitOfWork.GetRepository<ProjectTask>()
                    .GetAll().GroupBy(x => x.Status)
                    .Select(g => new GroupedTasksByStatus
                    {
                        Status = g.Key,
                        Tasks = g.Select(t=> new TaskPreview
                        {
                             Id= t.Id,      
                            Title = t.Title   
                        }).ToList() 
                    })
                    .ToList();
                    
            return RequestResult<IEnumerable<GroupedTasksByStatus>>.Success(groupedTasks, "Success");
        }

        private async Task<RequestResult<IEnumerable<GroupedTasksByStatus>>> ValidateRequest(GetTasksByStatusQuery request)
        {
            return RequestResult<IEnumerable<GroupedTasksByStatus>>.Success(default, "Success");
        }
    }

}
