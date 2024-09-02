using BadmintonSystem.Contract.Abstractions.Shared;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Messages;

// Return not res
public interface ICommand : IRequest<Result>
{
}

// Return => TResponse
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
