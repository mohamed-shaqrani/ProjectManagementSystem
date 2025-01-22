using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.ProjectsManagement.Projects.GetProject;

public class ProjectGroupKey
{
    public string Title { get; set; }
    public ProjectStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
