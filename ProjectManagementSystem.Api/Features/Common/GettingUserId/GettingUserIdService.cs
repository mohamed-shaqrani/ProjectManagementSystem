using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Repository;
using System.Security.Claims;

namespace ProjectManagementSystem.Api.Features.Common.GettingUserId
{
    public class GettingUserIdService : IGettingUserIdService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public GettingUserIdService(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork) 
        {
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
        }


        public async Task<int> GettingUserId() 
        {
            var repo =  _unitOfWork.GetRepository<User>();

            var useremail = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(useremail)) 
            {
                return 0;
            }
          var id = await repo.GetAll(u=> u.Email == useremail).Select(u => u.Id).FirstOrDefaultAsync();
            return id;
        }
    }
}
