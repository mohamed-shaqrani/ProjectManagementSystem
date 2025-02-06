using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask;

public class GetUserTasksResponseViewModel()
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ProjectTaskStatus Status { get; set; }
}

