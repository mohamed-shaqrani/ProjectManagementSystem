using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Features.Common
{
    public class UserTempData
    {
      public  string UserName { get; set; }

       public string Password { get; set; }

       public string Email { get; set; }

       public string Phone { get; set; }

        public Role Role { get; set; }
    }
}
