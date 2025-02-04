using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Extensions;
using ProjectManagementSystem.Api.Features.Common;
using ProjectManagementSystem.Api.Features.UserManagement.Queries;
using ProjectManagementSystem.Api.Helpers;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.UserManagement;

[Route("api/user/")]
public class GetUsersEndpoint : BaseEndpoint<UserParam, UserResponseViewModel>
{
    public GetUsersEndpoint(BaseEndpointParam<UserParam> param) : base(param)
    {

    }
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
