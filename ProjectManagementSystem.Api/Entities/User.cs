using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Api.Entities;
public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public string Phone { get; set; }
    public string ImagePath { get; set; }
    public ICollection<ProjectUserRoles> ProjectUserRoles = new List<ProjectUserRoles>();
    public DateTime PasswordResetCodeExpiration { get; set; }
    [StringLength(6)]
    public string? PasswordResetCode { get; set; }
}

