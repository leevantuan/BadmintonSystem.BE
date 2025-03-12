﻿using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Yard;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Yard;

public sealed class CreateYardCommandHandler(
    IMapper mapper,
    IRedisService redisService,
    IRepositoryBase<Domain.Entities.Yard, Guid> yardRepository)
    : ICommandHandler<Command.CreateYardCommand, Response.YardResponse>
{
    public async Task<Result<Response.YardResponse>> Handle
        (Command.CreateYardCommand request, CancellationToken cancellationToken)
    {
        await redisService.DeletesAsync("BMTSYS_");

        Domain.Entities.Yard? isNameExists =
            await yardRepository.FindSingleAsync(x => x.Name == request.Data.Name, cancellationToken);

        if (isNameExists != null)
        {
            return Result.Failure<Response.YardResponse>(new Error("400", "Name Exists!"));
        }

        Domain.Entities.Yard yard = mapper.Map<Domain.Entities.Yard>(request.Data);

        yard.IsStatus = (StatusEnum)request.Data.IsStatus;

        yardRepository.Add(yard);

        Response.YardResponse? result = mapper.Map<Response.YardResponse>(yard);

        return Result.Success(result);
    }
}
