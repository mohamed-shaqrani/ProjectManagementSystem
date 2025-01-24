using Api.Entities;
using MediatR;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Repository;

namespace ProjectManagementSystem.Api.Features.Common.Users.Queries
{
    public record HasAccessQuery(Feature Feature , Role Role) : IRequest<bool>;

    public class HasAccessQueryHandler : BaseRequestHandler<HasAccessQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public HasAccessQueryHandler(BaseRequestHandlerParam param, IUnitOfWork unitOfWork) 
            : base(param)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task<bool> Handle(HasAccessQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.GetRepository<RoleFeature>()
                .AnyAsync(x=>x.Role == request.Role && x.Feature == request.Feature);
        }
    }
}
