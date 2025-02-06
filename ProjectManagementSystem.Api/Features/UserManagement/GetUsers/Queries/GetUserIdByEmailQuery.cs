using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.UserManagement.GetUsers.Queries;

public record GetUserIdByEmailQuery(string email) : IRequest<RequestResult<int>>;
public class GetUserIdByEmailQueryHandler : BaseRequestHandler<GetUserIdByEmailQuery, RequestResult<int>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserIdByEmailQueryHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
    {
        _unitOfWork = unitOfWork;
    }
    public override async Task<RequestResult<int>> Handle(GetUserIdByEmailQuery request, CancellationToken cancellationToken)
    {

        var userId = _unitOfWork.GetRepository<User>().GetAll().Where(a => a.Email == request.email).Select(a => a.Id).First();



        return RequestResult<int>.Success(userId, "success");
    }

}
