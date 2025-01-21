using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Response;
using ProjectManagementSystem.Api.Response.Endpint;

namespace ProjectManagementSystem.Api.Features.Common;

[ApiController]
public class BaseEndpoint<TRequest, TResponse> : ControllerBase
{
    protected IValidator<TRequest> _validator;
    protected IMediator _mediator;

    public BaseEndpoint(BaseEndpointParam<TRequest> param)
    {
        _validator = param.Validator;
        _mediator = param.Mediator;

    }
    protected EndpointResponse<TResponse> ValidateRequest(TRequest request)
    {
        var validateResult = _validator.Validate(request);
        if (!validateResult.IsValid)
        {
            var errors = string.Join(",", validateResult.Errors.Select(x => x.ErrorMessage));
            return EndpointResponse<TResponse>.Failure(ErrorCode.ValidationError, errors);
        }
        return EndpointResponse<TResponse>.Success(default, "");
    }
}
