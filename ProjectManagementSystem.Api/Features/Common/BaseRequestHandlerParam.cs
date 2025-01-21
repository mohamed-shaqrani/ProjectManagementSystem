using MediatR;

namespace ProjectManagementSystem.Api.Features.Common;

public class BaseRequestHandlerParam
{
    readonly IMediator _mediator;

    public BaseRequestHandlerParam(IMediator mediator)
    {
        _mediator = mediator;
    }

    public IMediator Mediator => _mediator;

}
