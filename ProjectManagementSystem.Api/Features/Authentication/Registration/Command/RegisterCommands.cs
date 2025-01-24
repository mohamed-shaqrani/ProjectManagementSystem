using HotelManagement.Service.PasswordHasherServices;
using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.Authentication.Registration.Command
{
    public record RegisterCommand(string username, string email, string password) : IRequest<RequestResult<bool>>;
    public class RegisterHandler : BaseRequestHandler<RegisterCommand, RequestResult<bool>>
    {

        private readonly IUnitOfWork _unitOfWork;
        public RegisterHandler(BaseRequestHandlerParam requestHandlerParam, IUnitOfWork unitOfWork) : base(requestHandlerParam)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task<RequestResult<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<User>();
            var EmailAlreadyRegs = await repo.AnyAsync(u => u.Email == request.email);

            if (EmailAlreadyRegs)
            {
                return RequestResult<bool>.Failure(Response.ErrorCode.UserEmailExist, "f");
            }

            var password = PasswordHasherService.HashPassord(request.password);
            var role = Role.User;
            await repo.AddAsync(new User { Email = request.email, Password = password, Role = role, Username = request.username });
            await _unitOfWork.SaveChangesAsync();

            return RequestResult<bool>.Success(default, "s");

        }
    }
}
