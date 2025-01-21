using Api.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Api.Entities;

public class Project : BaseEntity
{
    [MaxLength(length: 500), Required]

    public string Title { get; set; }
    public ProjectStatus Status { get; set; }
    public ICollection<ProjectTask> Tasks = new List<ProjectTask>();
}
