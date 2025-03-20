﻿using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardPrice;

public sealed class UpdateYardPriceCommandHandler(
    IMapper mapper,
    IRedisService redisService,
    ICurrentTenantService currentTenantService,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : ICommandHandler<Command.UpdateYardPriceCommand, Response.YardPriceResponse>
{
    public async Task<Result<Response.YardPriceResponse>> Handle
        (Command.UpdateYardPriceCommand request, CancellationToken cancellationToken)
    {
        string endpoint = $"BMTSYS_{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        await redisService.DeletesAsync(endpoint);

        Domain.Entities.YardPrice yardPrice =
            await yardPriceRepository.FindByIdAsync(request.Data.Id, cancellationToken)
            ?? throw new YardPriceException.YardPriceNotFoundException(request.Data.Id);

        yardPrice.YardId = request.Data.YardId;
        yardPrice.PriceId = request.Data.PriceId ?? yardPrice.PriceId;
        yardPrice.TimeSlotId = request.Data.TimeSlotId;
        yardPrice.EffectiveDate = request.Data.EffectiveDate;
        yardPrice.IsBooking = (BookingEnum)request.Data.IsBooking;

        Response.YardPriceResponse? result = mapper.Map<Response.YardPriceResponse>(yardPrice);

        return Result.Success(result);
    }
}
