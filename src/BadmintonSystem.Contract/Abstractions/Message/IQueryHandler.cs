using BadmintonSystem.Contract.Abstractions.Shared;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Message;

public interface IQueryHandler<TQuery> : IRequestHandler<TQuery, Result>
    where TQuery : IQuery
{ }

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{ }
