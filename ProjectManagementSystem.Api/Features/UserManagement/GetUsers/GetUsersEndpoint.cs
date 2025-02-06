using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Entities;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.UserManagement.GetUsers.Queries;
using ProjectManagementSystem.Api.Filters;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.UserManagement.GetUsers;

[Route("api/user/")]
public class GetUsersEndpoint : BaseEndpoint<UserParam, UserResponseViewModel>
{
    public GetUsersEndpoint(BaseEndpointParam<UserParam> param) : base(param)
    {

    }
    [Authorize]
    [TypeFilter(typeof(CustomizeAuthorizeAttribute), Arguments = new object[] { Feature.ViewUsers })]

    [HttpGet]

    public async Task<ActionResult<EndpointResponse<PageList<UserResponseViewModel>>>> GetAll([FromQuery] UserParam projectParam)
    {
        var query = new GetUsersQuery(projectParam);
        var res = await _mediator.Send(query);
        Response.AddPaginationHeader(res.Data);

        return res.IsSuccess ? Ok(EndpointResponse<PageList<UserResponseViewModel>>.Success(res.Data, "Success"))
                             : StatusCode(500, EndpointResponse<PageList<UserResponseViewModel>>.Failure(res.ErrorCode, res.Message));

    }
}
