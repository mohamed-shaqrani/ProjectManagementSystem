using ProjectManagementSystem.Api.Entities;

namespace ProjectManagementSystem.Api.Filters;
public interface IRoleFeatureService
{
    Task<bool> HasAcess(Role role, Feature feature);
}
