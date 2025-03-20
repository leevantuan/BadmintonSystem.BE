﻿using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardType;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardType;

public sealed class UpdateYardTypeCommandHandler(
    IMapper mapper,
    IRedisService redisService,
    ICurrentTenantService currentTenantService,
    IRepositoryBase<Domain.Entities.YardType, Guid> yardTypeRepository)
    : ICommandHandler<Command.UpdateYardTypeCommand, Response.YardTypeResponse>
{
    public async Task<Result<Response.YardTypeResponse>> Handle
        (Command.UpdateYardTypeCommand request, CancellationToken cancellationToken)
    {
        string endpoint = $"BMTSYS_{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        await redisService.DeletesAsync(endpoint);

        Domain.Entities.YardType yardType = await yardTypeRepository.FindByIdAsync(request.Data.Id, cancellationToken)
                                            ?? throw new YardTypeException.YardTypeNotFoundException(request.Data.Id);

        Domain.Entities.YardType? isNameExists =
            await yardTypeRepository.FindSingleAsync(x => x.Name == request.Data.Name, cancellationToken);

        if (isNameExists != null)
        {
            return Result.Failure<Response.YardTypeResponse>(new Error("400", "Name Exists!"));
        }

        yardType.Name = request.Data.Name ?? yardType.Name;

        Response.YardTypeResponse? result = mapper.Map<Response.YardTypeResponse>(yardType);

        return Result.Success(result);
    }
}
