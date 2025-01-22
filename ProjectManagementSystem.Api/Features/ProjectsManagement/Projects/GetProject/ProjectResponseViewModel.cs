using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;

public class ProjectResponseViewModel
{
    public string Title { get; set; }
    public ProjectStatus Status { get; set; }
    public int NumTasks { get; set; }
    public DateTime DateCreated { get; set; }
}
