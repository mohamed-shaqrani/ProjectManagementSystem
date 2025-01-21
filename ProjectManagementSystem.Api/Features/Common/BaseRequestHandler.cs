using MediatR;

namespace ProjectManagementSystem.Api.Features.Common;

public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    protected IMediator _mediator;
    public BaseRequestHandler(BaseRequestHandlerParam requestHandlerParam)
    {
        _mediator = requestHandlerParam.Mediator;
    }

    public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);

}
