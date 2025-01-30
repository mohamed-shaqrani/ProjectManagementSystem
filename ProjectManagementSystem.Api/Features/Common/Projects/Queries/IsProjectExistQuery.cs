using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Repository;
using ProjectManagementSystem.Api.Response.RequestResult;

namespace ProjectManagementSystem.Api.Features.Common.Projects.Queries
{
    public record IsProjectExistQuery(int ProjectID): IRequest<RequestResult<bool>>;

    public class IsProjectExistQueryHandler : BaseRequestHandler<IsProjectExistQuery, RequestResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public IsProjectExistQueryHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) : base(param)
        {
            _unitOfWork = unitOfWork;
        }
        public override async Task<RequestResult<bool>> Handle(IsProjectExistQuery request, CancellationToken cancellationToken)
        {
            var isExist = await _unitOfWork.GetRepository<Project>().AnyAsync(x => x.Id == request.ProjectID);
            if (isExist)
            {
                return RequestResult<bool>.Success(true , "Project Exists correctly.");
            }
            return RequestResult<bool>.Success(false, "Project is not Exist.");
        }
    }
}
