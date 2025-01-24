using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Features.Common.Users.Queries;

namespace ProjectManagementSystem.Api.Filters
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        Feature _feature;
        IMediator _mediator;
        public CustomAuthorizeAttribute(Feature feature,IMediator mediator)
        {
            _feature = feature;
            _mediator = mediator;
        }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var roleID =  context.HttpContext.User.FindFirst(ClaimTypes.Role);
            if (roleID is null || string.IsNullOrEmpty(roleID.Value))
            {
                throw new UnauthorizedAccessException();
            }
            var role = (Role)int.Parse(roleID.Value);
            var hasAccess = await _mediator.Send(new HasAccessQuery(_feature, role));
            if (!hasAccess)
            {
                throw new UnauthorizedAccessException();
            }

            base.OnActionExecuting(context);
        }
    }
}
