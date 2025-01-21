using ProjectManagementSystem.Api.Entities;

namespace Api.Entities;
public class RoleFeature : BaseEntity
{
    public Role Role { get; set; }
    public Feature Feature { get; set; }
}

