using FluentValidation;
using MediatR;

namespace ProjectManagementSystem.Api.Features.Common;

public class BaseEndpointParam<TRequest>
{
    readonly IMediator _mediator;
    readonly IValidator<TRequest> _validator;
    public IMediator Mediator => _mediator;
    public IValidator<TRequest> Validator => _validator;
    public BaseEndpointParam(IMediator mediator, IValidator<TRequest> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }
}
