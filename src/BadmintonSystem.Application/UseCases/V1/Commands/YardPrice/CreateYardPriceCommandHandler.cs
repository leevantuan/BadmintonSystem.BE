using AutoMapper;
using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Application.UseCases.V1.Commands.YardPrice;

public sealed class CreateYardPriceCommandHandler(
    IMapper mapper,
    IRedisService redisService,
    ICurrentTenantService currentTenantService,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : ICommandHandler<Command.CreateYardPriceCommand, Response.YardPriceResponse>
{
    public async Task<Result<Response.YardPriceResponse>> Handle
        (Command.CreateYardPriceCommand request, CancellationToken cancellationToken)
    {
        string endpoint = $"BMTSYS_{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        await redisService.DeletesAsync(endpoint);

        Domain.Entities.YardPrice yardPrice = mapper.Map<Domain.Entities.YardPrice>(request.Data);

        yardPrice.IsBooking = (BookingEnum)request.Data.IsBooking;

        yardPriceRepository.Add(yardPrice);

        Response.YardPriceResponse? result = mapper.Map<Response.YardPriceResponse>(yardPrice);

        return Result.Success(result);
    }
}
