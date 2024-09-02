using BadmintonSystem.Contract.Abstractions.Shared;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Messages;

// Nhận vào TQuery và trả về TResponse => IQueryHandler<TQuery, TResponse>
// Trả về truyền vào Resullt => IRequestHandler<TQuery, Result<TResponse>>
// Where là truyền vào contain nó kh hiểu IQuery là gì ở => IQueryHandler<TQuery, TResponse>
// Điều kiện cái IQuery là cái IQuery vừa tạo ở File => IQuery.cs
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
