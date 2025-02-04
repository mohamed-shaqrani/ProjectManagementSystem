using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Api.Entities;
public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }

    public string Phone { get; set; } = string.Empty;
    public string? ImagePath { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public bool IsAuthenticated { get; set; }



    public ICollection<ProjectUserRoles> ProjectUserRoles = new List<ProjectUserRoles>();
    public ICollection<ProjectTask> Tasks = new List<ProjectTask>();

    public DateTime PasswordResetCodeExpiration { get; set; }
    [StringLength(6)]
    public string? PasswordResetCode { get; set; } = string.Empty;
}

