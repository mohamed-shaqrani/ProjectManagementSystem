namespace ProjectManagementSystem.Api.Entities;

public class TempAuthCode : BaseEntity
{
    public int Code { get; set; }
    public string Email { get; set; }
    public DateTime ExpiresOn { get; set; }
    public bool IsUsed { get; set; }
    public DateTime UsedOn { get; set; }
}
