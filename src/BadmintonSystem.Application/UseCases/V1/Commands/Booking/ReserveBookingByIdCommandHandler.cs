using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using MassTransit;
using MediatR;
using Newtonsoft.Json;
using Response = BadmintonSystem.Contract.Services.V1.Booking.Response;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class ReserveBookingByIdCommandHandler(
    IBookingHub bookingHub,
    IBus bus,
    ISender sender,
    ICurrentTenantService currentTenantService,
    IRedisService redisService,
    IRepositoryBase<Domain.Entities.YardPrice, Guid> yardPriceRepository)
    : ICommandHandler<Command.ReserveBookingByIdCommand>
{
    public async Task<Result> Handle(Command.ReserveBookingByIdCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.YardPrice yardPrices = await yardPriceRepository.FindByIdAsync(request.Id, cancellationToken)
                                              ?? throw new YardPriceException.YardPriceNotFoundException(request.Id);

        var ids = new List<Guid> { request.Id };
        var idByDate = new Response.GetIdsByDate
        {
            Ids = ids,
            Date = yardPrices.EffectiveDate
        };

        var idsByDate = new List<Response.GetIdsByDate> { idByDate };

        var sendSignalRAndUpdateCache = new BusCommand.SendUpdateCacheBusCommand
        {
            Id = Guid.NewGuid(),
            Description = request.Data.IsToken,
            Name = "Email Notification",
            TimeSpan = DateTime.Now,
            TransactionId = Guid.NewGuid(),
            YardPriceIds = idsByDate,
            Type = request.Data.Type.ToUpper() == "RESERVED"
                ? BookingEnum.RESERVED.ToString()
                : BookingEnum.UNBOOKED.ToString()
        };

        foreach (Response.GetIdsByDate yardPrice in idsByDate)
        {
            // Real-time SignalR
            await bookingHub.BookingByUserAsync(new Contract.Services.V1.Bill.Response.BookingHubResponse
            {
                Ids = yardPrice.Ids,
                Type = request.Data.Type.ToUpper() == "RESERVED"
                ? BookingEnum.RESERVED.ToString()
                : BookingEnum.UNBOOKED.ToString()
            });

            // UPDATE CACHE
            string endpoint = $"{currentTenantService.Name}-get-yard-prices-by-date";
            //string endpoint = "get-yard-prices-by-date";
            string cacheKey = StringExtension.GenerateCacheKeyFromRequest(endpoint, yardPrice.Date);
            string yardPriceJson = await redisService.GetAsync(cacheKey);
            if (string.IsNullOrEmpty(yardPriceJson))
            {
                await sender.Send(
                    new Contract.Services.V1.YardPrice.Query.GetYardPricesByDateQuery(Guid.Empty, yardPrice.Date, currentTenantService.Name),
                    cancellationToken);
                yardPriceJson = await redisService.GetAsync(cacheKey);
            }

            // Convert
            List<Contract.Services.V1.YardPrice.Response.YardPricesByDateDetailResponse>? data =
                JsonConvert
                    .DeserializeObject<
                        List<Contract.Services.V1.YardPrice.Response.YardPricesByDateDetailResponse>>(
                        yardPriceJson);

            // Delete Cache
            await redisService.DeleteByKeyAsync(cacheKey);

            // Update Data
            if (data != null)
            {
                var detailsToUpdate = data.SelectMany(x => x.YardPricesDetails)
                    .Where(detail => yardPrice.Ids.Contains(detail.Id))
                    .ToList();

                if (request.Data.Type != null && detailsToUpdate.Count != 0)
                {
                    BookingEnum bookingType = request.Data.Type.ToUpperInvariant() switch
                    {
                        var type when type == BookingEnum.BOOKED.ToString() => BookingEnum.BOOKED,
                        var type when type == BookingEnum.RESERVED.ToString() => BookingEnum.RESERVED,
                        _ => BookingEnum.UNBOOKED
                    };

                    foreach (Contract.Services.V1.YardPrice.Response.YardPricesByDateDetail detail in
                             detailsToUpdate)
                    {
                        detail.IsBooking = (int)bookingType;
                        detail.ExpirationTime = bookingType == BookingEnum.RESERVED
                            ? DateTime.UtcNow.AddSeconds(30)
                            : null;
                        detail.IsToken = bookingType == BookingEnum.RESERVED ? request.Data.IsToken : string.Empty;
                    }

                    await redisService.SetAsync(cacheKey, data, TimeSpan.FromMinutes(30));
                }
            }
        }

        //ISendEndpoint endPoint = await bus.GetSendEndpoint(new Uri("queue:send-update-cache-queue"));
        //await endPoint.Send(sendSignalRAndUpdateCache, cancellationToken);
        //string endpoint = $"BMTSYS_{currentTenantService.Code.ToString()}-get-yard-prices-by-date";
        //await redisService.DeletesAsync(endpoint);

        return Result.Success();
    }
}
