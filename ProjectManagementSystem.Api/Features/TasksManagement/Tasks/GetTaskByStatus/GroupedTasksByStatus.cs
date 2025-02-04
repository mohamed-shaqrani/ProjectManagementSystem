using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTaskByStatus
{
    public class GroupedTasksByStatus
    {
        public ProjectTaskStatus Status { get; set; }
        public List<TaskPreview> Tasks { get; set; }
    }

    public class TaskPreview
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
