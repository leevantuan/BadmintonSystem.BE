using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Enumerations;
using MediatR;
using Microsoft.Extensions.Logging;
using Query = BadmintonSystem.Contract.Services.V1.YardPrice.Query;

namespace BadmintonSystem.Infrastructure.Bus.UseCase.Commands;

public sealed class SendUpdateCacheBusCommandConsumerHandler(
    IBookingHub bookingHub,
    IRedisService redisService,
    ISender sender,
    ILogger<BusCommand.SendUpdateCacheBusCommand> logger)
    : IRequestHandler<BusCommand.SendUpdateCacheBusCommand>
{
    public async Task Handle(BusCommand.SendUpdateCacheBusCommand request, CancellationToken cancellationToken)
    {
        foreach (Response.GetIdsByDate yardPrice in request.YardPriceIds)
        {
            // Real-time SignalR
            await bookingHub.BookingByUserAsync(new Contract.Services.V1.Bill.Response.BookingHubResponse
            {
                Ids = yardPrice.Ids,
                Type = BookingEnum.BOOKED.ToString()
            });

            // Update Redis cache
            string endpoint = "get-yard-prices-by-date";
            string cacheKey = StringExtension.GenerateCacheKeyFromRequest(endpoint, yardPrice.Date);
            await redisService.DeleteByKeyAsync(cacheKey);

            _ = await sender.Send(new Query.GetYardPricesByDateQuery(Guid.Empty, yardPrice.Date));
        }
    }
}
