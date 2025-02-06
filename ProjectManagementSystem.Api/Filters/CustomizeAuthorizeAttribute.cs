using Microsoft.AspNetCore.Mvc.Filters;
using ProjectManagementSystem.Api.Entities;
using System.Security.Claims;

namespace ProjectManagementSystem.Api.Filters;

public class CustomizeAuthorizeAttribute : ActionFilterAttribute
{
    private readonly IRoleFeatureService _roleFeatureService;
    Feature _feature;
    public CustomizeAuthorizeAttribute(Feature feature, IRoleFeatureService roleFeatureService)
    {
        _roleFeatureService = roleFeatureService;
        _feature = feature;
    }
    public override async void OnActionExecuted(ActionExecutedContext context)
    {
        var claims = context.HttpContext.User;
        var roleId = claims.FindFirst(ClaimTypes.Role);
        if (roleId == null || string.IsNullOrEmpty(roleId.Value))
        {
            throw new UnauthorizedAccessException();
        }
        Enum.TryParse<Role>(roleId.Value, out var role);
        if (!await _roleFeatureService.HasAcess(role, _feature))
        {
            throw new UnauthorizedAccessException();


        }
        if (!await _roleFeatureService.IsUserActive(claims.FindFirst(ClaimTypes.Email).Value))
        {
            throw new UnauthorizedAccessException("This account is non Active");


        }
    }

}
