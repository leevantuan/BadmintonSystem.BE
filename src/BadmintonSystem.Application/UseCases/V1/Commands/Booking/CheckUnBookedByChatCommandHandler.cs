using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Enumerations;
using static BadmintonSystem.Contract.Services.V1.TimeSlot.Response;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class CheckUnBookedByChatCommandHandler(
    ICurrentUserInfoService currentUserInfoService,
    IYardPriceService yardPriceService)
    : ICommandHandler<Command.CheckUnBookedByChatCommand>
{
    public async Task<Result> Handle(Command.CheckUnBookedByChatCommand request, CancellationToken cancellationToken)
    {
        var yardPricesUnBooked = await IsUnBooked(request.Data.BookingDate, request.Data.StartTime, request.Data.EndTime, request.Data.Tenant, cancellationToken);

        if (yardPricesUnBooked == null || !yardPricesUnBooked.Any())
        {
            throw new ApplicationException("YardPrice is not available");
        }

        return Result.Success(yardPricesUnBooked);
    }

    private async Task<List<TimeSlotWithYardPriceResponse>> IsUnBooked(
        DateTime bookingDate,
        TimeSpan startTime,
        TimeSpan endTime,
        string tenant,
        CancellationToken cancellationToken)
    {
        var timeSlots = new List<TimeSlotWithYardPriceResponse>();

        List<Contract.Services.V1.YardPrice.Response.YardPricesByDateDetailResponse> yardPrices =
                   await yardPriceService.GetYardPrices(
                            bookingDate,
                            tenant,
                            currentUserInfoService.UserId ?? Guid.Empty,
                            cancellationToken);

        if (yardPrices == null || !yardPrices.Any())
        {
            return timeSlots;
        }

        //var timeSlots = new Dictionary<Guid, Guid>();

        foreach (var yardPrice in yardPrices)
        {
            var filterDetails = yardPrice.YardPricesDetails
                .Where(x => x.IsBooking == (int)BookingEnum.UNBOOKED
                    && x.StartTime >= startTime && x.EndTime <= endTime)
                .ToList();

            foreach (var detail in filterDetails)
            {
                if (!timeSlots.Any(t => t.Id == detail.TimeSlotId))
                {
                    timeSlots.Add(new TimeSlotWithYardPriceResponse
                    {
                        Id = detail.TimeSlotId,
                        YardPriceId = detail.Id,
                        StartTime = detail.StartTime,
                        EndTime = detail.EndTime,
                    });
                }
            }
        }

        return timeSlots;
    }
}
