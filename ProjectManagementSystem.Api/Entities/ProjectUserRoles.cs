using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Api.Entities
{
    public class ProjectUserRoles : BaseEntity
    {
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public Role Role { get; set; }
    }
}
