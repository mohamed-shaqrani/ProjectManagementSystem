using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectManagementSystem.Api.Entities;
using System.Security.Claims;

namespace ProjectManagementSystem.Api.Filters;

public class CustomizeAuthorizeAttribute : ActionFilterAttribute
{
    private readonly IRoleFeatureService _roleFeatureService;
    private readonly Feature _feature;

    public CustomizeAuthorizeAttribute(Feature feature, IRoleFeatureService roleFeatureService)
    {
        _roleFeatureService = roleFeatureService;
        _feature = feature;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claims = context.HttpContext.User;
        var roleId = claims.FindFirst(ClaimTypes.Role);

        if (roleId == null || string.IsNullOrEmpty(roleId.Value))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        Enum.TryParse<Role>(roleId.Value, out var role);

        if (!await _roleFeatureService.HasAcess(role, _feature))
        {
            context.Result = new ForbidResult();
            return;
        }

        var emailClaim = claims.FindFirst(ClaimTypes.Email);
        if (emailClaim == null || !await _roleFeatureService.IsUserActive(emailClaim.Value))
        {
            context.Result = new UnauthorizedObjectResult("This account is non-active");
            return;
        }

        await next(); 
    }
}
