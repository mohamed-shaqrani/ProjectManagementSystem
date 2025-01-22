using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Api.Entities;
public class ProjectTask : BaseEntity
{
    [MaxLength(length: 500), Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public ProjectTaskStatus Status { get; set; }
    public int UserID { get; set; }
    public User User { get; set; }
}
