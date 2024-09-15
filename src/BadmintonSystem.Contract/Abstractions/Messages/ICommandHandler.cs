﻿using BadmintonSystem.Contract.Abstractions.Shared;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Messages;
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
