using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;


namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask.Queries
{
    public record GetTasksQuery():IRequest<RequestResult<IEnumerable<TaskDTO>>>;

    public class GetTasksQueryHandler : BaseRequestHandler<GetTasksQuery, RequestResult<IEnumerable<TaskDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTasksQueryHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
        {
            _unitOfWork = unitOfWork;
        }
        public override async Task<RequestResult<IEnumerable<TaskDTO>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var response = await ValidateRequest(request);
            if (!response.IsSuccess)
            {
                return response;
            }

            var projects = _unitOfWork.GetRepository<ProjectTask>()
                    .GetAll()
                    .Select(
                        x => new TaskDTO {
                            Title = x.Title,
                            Status = x.Status,
                            UserName = x.User.Username,
                            ProjectName = x.Project.Title,
                            DateCreated = x.CreatedAt,
                        }
                    ).ToList();
            return RequestResult<IEnumerable<TaskDTO>>.Success(projects, "Success");
        }

        private async Task<RequestResult<IEnumerable<TaskDTO>>> ValidateRequest(GetTasksQuery request)
        {
            return RequestResult<IEnumerable<TaskDTO>>.Success(default, "Success");
        }
    }

}
