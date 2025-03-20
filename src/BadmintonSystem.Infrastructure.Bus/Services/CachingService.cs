using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Enumerations;
using MediatR;
using Newtonsoft.Json;
using Query = BadmintonSystem.Contract.Services.V1.YardPrice.Query;

namespace BadmintonSystem.Infrastructure.Bus.Services;

public class CachingService(
    IBookingHub bookingHub,
    ISender sender,
    IRedisService redisService)
    : ICachingService
{
    public async Task SendUpdateCachingAsync
        (BusCommand.SendUpdateCacheBusCommand request, CancellationToken cancellationToken = default)
    {
        try
        {
            foreach (Response.GetIdsByDate yardPrice in request.YardPriceIds)
            {
                // Real-time SignalR
                await bookingHub.BookingByUserAsync(new Contract.Services.V1.Bill.Response.BookingHubResponse
                {
                    Ids = yardPrice.Ids,
                    Type = request.Type
                });

                // UPDATE CACHE
                string endpoint = $"{request.Name}-get-yard-prices-by-date";
                //string endpoint = "get-yard-prices-by-date";
                string cacheKey = StringExtension.GenerateCacheKeyFromRequest(endpoint, yardPrice.Date);
                string yardPriceJson = await redisService.GetAsync(cacheKey);
                if (string.IsNullOrEmpty(yardPriceJson))
                {
                    await sender.Send(
                        new Query.GetYardPricesByDateQuery(Guid.Empty, yardPrice.Date, request.Name),
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

                    if (request.Type != null && detailsToUpdate.Count != 0)
                    {
                        BookingEnum bookingType = request.Type.ToUpperInvariant() switch
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
                            detail.IsToken = bookingType == BookingEnum.RESERVED ? request.Description : string.Empty;
                        }

                        await redisService.SetAsync(cacheKey, data, TimeSpan.FromMinutes(30));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occured while processing your request: {ex.Message}");
        }
    }
}
