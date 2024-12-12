using BadmintonSystem.Contract.Abstractions.Shared;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Message;

public interface IQuery : IRequest<Result>
{ }

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{ }
