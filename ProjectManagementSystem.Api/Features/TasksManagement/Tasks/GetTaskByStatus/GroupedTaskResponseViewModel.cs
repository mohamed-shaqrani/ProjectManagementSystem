using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTaskByStatus
{
    public class GroupedTaskResponseViewModel
    {
        public ProjectTaskStatus Status { get; set; }
        public List<TaskPreviewViewModel> Tasks { get; set; }
    }

    public class TaskPreviewViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

}
