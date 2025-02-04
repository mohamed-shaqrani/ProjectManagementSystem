using Api.Entities;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Repository;

namespace ProjectManagementSystem.Api.Filters;

public class RoleFeatureService : IRoleFeatureService
{
    private readonly IUnitOfWork _UnitOfWork;
    public RoleFeatureService(IUnitOfWork unitOfWork)
    {
        _UnitOfWork = unitOfWork;
    }
    public async Task<bool> HasAcess(Role role, Feature feature)
    {
        var result = await _UnitOfWork.GetRepository<RoleFeature>().AnyAsync(x => x.Role == role && x.Feature == feature);
        return result;
    }

}
