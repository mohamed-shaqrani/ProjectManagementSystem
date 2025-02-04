using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.TasksManagement.Tasks.GetTask
{
    public class TaskResponseViewModel
    {

        public string Title { get; set; }
        public ProjectTaskStatus Status { get; set; }
        public string UserName { get; set; }
        public string ProjectName { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
