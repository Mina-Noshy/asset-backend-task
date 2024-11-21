using MediatR;

namespace Asset.Application.Abstractions;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
