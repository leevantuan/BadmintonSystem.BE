using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Extensions;
using BadmintonSystem.Contract.Services.V1.YardPrice;
using BadmintonSystem.Domain.Enumerations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace BadmintonSystem.Infrastructure.Background;

public class BookingExpirationWorker(
    IServiceScopeFactory scopeFactory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (IServiceScope scope = scopeFactory.CreateScope()) // 🔹 Tạo scope để lấy Scoped services
            {
                IRedisService redisService = scope.ServiceProvider.GetRequiredService<IRedisService>();
                IBookingHub bookingHub = scope.ServiceProvider.GetRequiredService<IBookingHub>();
                var currentTenantService = scope.ServiceProvider.GetRequiredService<ICurrentTenantService>();

                string endpoint = "get-yard-prices-by-date";
                DateTime today = DateTime.Now;
                DateTime tomorrow = today.AddDays(1);
                DateTime dayAfterTomorrow = today.AddDays(2);
                var dateList = new List<DateTime> { today, tomorrow, dayAfterTomorrow };
                foreach (DateTime date in dateList)
                {
                    string cacheKey = StringExtension.GenerateCacheKeyFromRequest(endpoint, date);

                    List<string> yardPriceJsons = await redisService.GetBeforeAsync(cacheKey);

                    foreach (string yardPriceJson in yardPriceJsons)
                    {
                        await UpdateCachingAsync(yardPriceJson, cacheKey, bookingHub, redisService);
                    }
                }
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private static async Task UpdateCachingAsync
        (string yardPriceJson, string cacheKey, IBookingHub bookingHub, IRedisService redisService)
    {
        if (!string.IsNullOrEmpty(yardPriceJson))
        {
            List<Response.YardPricesByDateDetailResponse> bookingList =
                JsonConvert
                    .DeserializeObject<List<Response.YardPricesByDateDetailResponse>>(yardPriceJson) ??
                new List<Response.YardPricesByDateDetailResponse>();

            var detailsToUpdate = bookingList.SelectMany(x => x.YardPricesDetails)
                .Where(detail => detail.IsBooking == (int)BookingEnum.RESERVED)
                .ToList();

            bool isUpdated = false;
            foreach (Response.YardPricesByDateDetail booking in detailsToUpdate)
            {
                if (booking.ExpirationTime < DateTime.UtcNow)
                {
                    booking.IsBooking = (int)BookingEnum.UNBOOKED;
                    booking.ExpirationTime = null;
                    booking.IsToken = string.Empty;
                    isUpdated = true;

                    // Gửi thông báo qua SignalR
                    await bookingHub.BookingByUserAsync(
                        new Contract.Services.V1.Bill.Response.BookingHubResponse
                        {
                            Ids = new List<Guid> { booking.Id },
                            Type = BookingEnum.UNBOOKED.ToString()
                        });
                }
            }

            // Nếu có thay đổi trạng thái, cập nhật lại Redis
            if (isUpdated)
            {
                await redisService.SetAsync(cacheKey, bookingList, TimeSpan.FromMinutes(30));
            }
            else
            {
                await redisService.SetAsync(cacheKey, bookingList, TimeSpan.FromMinutes(30));
            }
        }
    }
}
