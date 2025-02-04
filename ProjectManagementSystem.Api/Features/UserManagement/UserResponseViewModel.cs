namespace ProjectManagementSystem.Api.Features.UserManagement;

public class UserResponseViewModel
{
    public int Id { get; set; }

    public bool IsActive { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public DateTime CreatedAt { get; set; }
}
