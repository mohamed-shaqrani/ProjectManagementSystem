namespace ProjectManagementSystem.Api.Features.UserManagement.GetUsers;

public class UserResponseViewModel
{
    public int Id { get; set; }

    public bool IsActive { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public int Role { get; set; }

    public DateTime CreatedAt { get; set; }
}
