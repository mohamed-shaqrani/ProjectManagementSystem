using AutoMapper.QueryableExtensions;
using MediatR;
using PredicateExtensions;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.MappingProfiles;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;
using System.Linq.Expressions;

namespace ProjectManagementSystem.Api.Features.UserManagement.Queries;

public record GetUsersQuery(UserParam UserParam) : IRequest<RequestResult<PageList<UserResponseViewModel>>>;
public class GetUsersHandler : BaseRequestHandler<GetUsersQuery, RequestResult<PageList<UserResponseViewModel>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUsersHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
    {
        _unitOfWork = unitOfWork;
    }
    public override async Task<RequestResult<PageList<UserResponseViewModel>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var predicate = BuildPredicate(request.UserParam);

        var query = _unitOfWork.GetRepository<User>().GetAll(predicate).ProjectTo<UserResponseViewModel>();


        var paginatedResult = await PageList<UserResponseViewModel>.CreateAsync(query, request.UserParam.PageNumber, request.UserParam.PageSize);

        return RequestResult<PageList<UserResponseViewModel>>.Success(paginatedResult, "success");
    }
    private Expression<Func<User, bool>> BuildPredicate(UserParam request)
    {
        var predicate = PredicateExtensions.PredicateExtensions.Begin<User>(true);
        if (!string.IsNullOrEmpty(request.Name))

            predicate = predicate.And(p => p.Username.Contains(request.Name));
        if (!string.IsNullOrEmpty(request.Email))

            predicate = predicate.And(p => p.Email.Contains(request.Email));


        return predicate;
    }
}
