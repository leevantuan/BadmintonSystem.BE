﻿using BadmintonSystem.Contract.Abstractions.Shared;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Message;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
