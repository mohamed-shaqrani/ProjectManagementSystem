using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;

public class ProjectResponseViewModel
{
    public int ProjectId { get; set; }

    public string Title { get; set; }
    public ProjectStatus Status { get; set; }
    public int NumTasks { get; set; }
    public int NumOfUsers { get; set; }

    public DateTime DateCreated { get; set; }
}
