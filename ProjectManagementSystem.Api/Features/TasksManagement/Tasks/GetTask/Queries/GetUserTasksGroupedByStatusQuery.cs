using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask.Queries;
public record GetUserTasksGroupedByStatusQuery(int UserID) : IRequest<RequestResult<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>>;

public class GetUserTasksGroupedByStatusHandler : BaseRequestHandler<GetUserTasksGroupedByStatusQuery, RequestResult<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserTasksGroupedByStatusHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
    {
        _unitOfWork = unitOfWork;
    }
    public override async Task<RequestResult<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>> Handle(GetUserTasksGroupedByStatusQuery request, CancellationToken cancellationToken)
    {
        var userTaskList = await _unitOfWork.GetRepository<ProjectTask>().GetAll()
                                                            .Where(a => a.UserID == request.UserID)
                                                            .GroupBy(a => a.Status)
                                                            .ToDictionaryAsync(a => a.Key, g => g.Select(task =>
                                                            new GetUserTasksResponseViewModel
                                                            {
                                                                Description = task.Description,
                                                                Title = task.Title,
                                                                Status = task.Status
                                                            }).ToList(), cancellationToken);

        return RequestResult<Dictionary<ProjectTaskStatus, List<GetUserTasksResponseViewModel>>>.Success(userTaskList, "Success");
    }


}


