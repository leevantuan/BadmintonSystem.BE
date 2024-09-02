using BadmintonSystem.Contract.Abstractions.Shared;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Messages;

// Query required response
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
