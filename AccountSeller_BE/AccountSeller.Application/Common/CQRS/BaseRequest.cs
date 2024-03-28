using MediatR;

namespace AccountSeller.Application.Common.CQRS
{
    public abstract class BaseRequest<TResponse> : IRequest<TResponse> where TResponse : class
    {
    }

    public abstract class BaseRequest : IRequest<Unit>
    {
    }
}
