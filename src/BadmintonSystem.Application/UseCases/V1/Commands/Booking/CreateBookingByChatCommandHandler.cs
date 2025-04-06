using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Services;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static BadmintonSystem.Contract.Services.V1.TimeSlot.Response;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class CreateBookingByChatCommandHandler(
    ApplicationDbContext context,
    TenantDbContext tenantContext,
    IBookingService bookingService,
    IMediator mediator,
    IRedisService redisService,
    ICurrentTenantService currentTenantService,
    ICurrentUserInfoService currentUserInfoService,
    IYardPriceService yardPriceService,
    IMomoService momoService,
    IBookingLineService bookingLineService)
    : ICommandHandler<Command.CreateBookingByChatCommand>
{
    public async Task<Result> Handle(Command.CreateBookingByChatCommand request, CancellationToken cancellationToken)
    {
        var yardPricesUnBooked = await IsUnBooked(request.Data.BookingDate, request.Data.StartTime, request.Data.EndTime, request.Data.Tenant, cancellationToken);

        if (yardPricesUnBooked == null || !yardPricesUnBooked.Any())
        {
            return Result.Failure(new Error("YardPrice", "YardPrice is not available"));
        }

        var YardPriceIds = yardPricesUnBooked
            .Select(y => y.YardPriceId)
            .ToList();

        List<Contract.Services.V1.Booking.Response.GetIdsByDate> idsByDate =
                await GetIdsByDateAsync(YardPriceIds, cancellationToken);

        var bookingIds = new List<Guid>();
        decimal totalPrice = 0;
        foreach (Contract.Services.V1.Booking.Response.GetIdsByDate idByDate in idsByDate)
        {
            var bookingEntity = new Domain.Entities.Booking
            {
                Id = Guid.NewGuid(),
                BookingStatus = BookingStatusEnum.Pending,
                PaymentStatus = PaymentStatusEnum.Unpaid,
                UserId = currentUserInfoService.UserId ?? Guid.NewGuid(),
                BookingDate = idByDate.Date,
                BookingTotal = 0,
                OriginalPrice = 0,
                PercentPrePay = 100,
                SaleId = null,
                FullName = currentUserInfoService.UserName ?? "User",
                PhoneNumber = "0000000000"
            };

            var billEntity = new Domain.Entities.Bill
            {
                Id = Guid.NewGuid(),
                UserId = bookingEntity.UserId,
                BookingId = bookingEntity.Id,
                TotalPrice = 0,
                Status = BillStatusEnum.BOOKED
            };

            totalPrice = await CreateBookingAsync(idByDate, bookingEntity, billEntity, cancellationToken);

            bookingIds.Add(bookingEntity.Id);
        }

        await SignalRAndUpdateCacheAsync(idsByDate, request.Data.Tenant, cancellationToken);
        await SendMailAsync(bookingIds, currentUserInfoService.UserName, request.Data.Email, NotificationType.client, cancellationToken);
        await SendMailAsync(bookingIds, currentUserInfoService.UserName, request.Data.Email, NotificationType.staff, cancellationToken);

        string endpoint = $"BMTSYS_{request.Data.Tenant}-get-yard-prices-by-date";

        await redisService.DeletesAsync(endpoint);

        // Create QRCode
        var momoRequest = new Contract.Services.V1.Momo.Request.MomoPaymentRequest()
        {
            OrderId = bookingIds.FirstOrDefault().ToString(),
            Amount = totalPrice.ToString(),
            OrderInfo = currentTenantService.Name.ToString(),
        };

        try
        {
            string qrCodeUrl = await momoService.CreatePaymentQRCodeAsync(momoRequest, cancellationToken);

            var response = new Contract.Services.V1.Momo.Response.MomoPaymentResponse
            {
                ResultCode = 0,
                Message = "QR Code generated successfully.",
                QrCodeUrl = "QRCode",
                PayUrl = qrCodeUrl
            };
            return Result.Success(response);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Failed to create QR Code", ex);
        }
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

    private async Task SendMailAsync
    (List<Guid> bookingIds, string fullName, string email, string type, CancellationToken cancellationToken)
    {

        email = type == NotificationType.staff ? "managersystem.net@gmail.com" : email;

        if (type == NotificationType.client)
        {
            await mediator.Publish(
                new DomainEvent.BookingDone(bookingIds, fullName, email),
                cancellationToken);
        }

        // SEND_MAIL Notification Staff
        if (type == NotificationType.staff)
        {
            await mediator.Publish(
                new DomainEvent.BookingNotificationToStaff(bookingIds, fullName, email),
                cancellationToken);
        }

        //ISendEndpoint endPoint = await bus.GetSendEndpoint(new Uri($"queue:{queue}"));
        //await endPoint.Send(sendEmailClient, cancellationToken);
    }

    private async Task SignalRAndUpdateCacheAsync
        (List<Contract.Services.V1.Booking.Response.GetIdsByDate> idsByDate, string tenant, CancellationToken cancellationToken)
    {
        var sendSignalRAndUpdateCache = new BusCommand.SendUpdateCacheBusCommand
        {
            Id = Guid.NewGuid(),
            Description = "Email Description",
            Name = tenant,
            TimeSpan = DateTime.Now,
            TransactionId = Guid.NewGuid(),
            YardPriceIds = idsByDate,
            Type = BookingEnum.BOOKED.ToString()
        };

        await bookingService.SignalRAndUpdateCacheAsync(sendSignalRAndUpdateCache, cancellationToken);
        //ISendEndpoint endPoint = await bus.GetSendEndpoint(new Uri("queue:send-update-cache-queue"));
        //await endPoint.Send(sendSignalRAndUpdateCache, cancellationToken);
    }

    private async Task<List<Contract.Services.V1.Booking.Response.GetIdsByDate>> GetIdsByDateAsync
        (List<Guid> ids, CancellationToken cancellationToken)
    {
        List<Contract.Services.V1.Booking.Response.GetIdsByDate> yardPrice = await context.YardPrice
            .Where(x => ids.Contains(x.Id))
            .GroupBy(x => x.EffectiveDate.Date)
            .Select(x => new Contract.Services.V1.Booking.Response.GetIdsByDate
            {
                Date = x.Key,
                Ids = x.Select(y => y.Id).ToList()
            }).ToListAsync(cancellationToken);

        return yardPrice;
    }

    private async Task<decimal> CreateBookingAsync
    (Contract.Services.V1.Booking.Response.GetIdsByDate yardPriceIds, Domain.Entities.Booking bookingEntity, Domain.Entities.Bill billEntity,
        CancellationToken cancellationToken)
    {
        context.Booking.Add(bookingEntity);
        await context.SaveChangesAsync(cancellationToken);

        context.Bill.Add(billEntity);
        await context.SaveChangesAsync(cancellationToken);

        decimal originalPrice =
            await bookingLineService.CreateBookingLines(bookingEntity.Id, yardPriceIds.Ids, cancellationToken);
        decimal totalPrice = originalPrice;

        if (bookingEntity.SaleId != null)
        {
            Domain.Entities.Sale percentSale =
                await context.Sale.FirstOrDefaultAsync(s => s.Id == bookingEntity.SaleId, cancellationToken)
                ?? throw new SaleException.SaleNotFoundException(bookingEntity.SaleId
                    .Value);

            totalPrice -= totalPrice * percentSale.Percent / 100;
        }

        bookingEntity.BookingTotal = totalPrice;
        bookingEntity.OriginalPrice = originalPrice;
        billEntity.TotalPrice = totalPrice;
        billEntity.TotalPayment = totalPrice * bookingEntity.PercentPrePay / 100;
        billEntity.Name = bookingEntity.FullName;

        await context.SaveChangesAsync(cancellationToken);

        // Class BookingHistory
        var yardPriceIdsResult = await context.YardPrice.Where(x => yardPriceIds.Ids.Contains(x.Id))
            .Select(x => new
            {
                x.TimeSlotId
            })
            .ToListAsync(cancellationToken);

        var timeSlotIds = yardPriceIdsResult.Select(x => x.TimeSlotId).ToList();

        var startTimeFirst = await context.TimeSlot
            .Where(x => timeSlotIds.Contains(x.Id))
            .OrderBy(x => x.StartTime)
            .Select(x => x.StartTime)
            .FirstOrDefaultAsync(cancellationToken);

        var bookingHistoryEntities = new Domain.Entities.BookingHistory()
        {
            Id = Guid.NewGuid(),
            BookingId = bookingEntity.Id,
            PaymentStatus = PaymentStatusEnum.Paid,
            CreatedDate = DateTime.Now,
            UserId = bookingEntity.UserId,
            ClubName = currentTenantService.Name.ToString(),
            StartTime = startTimeFirst,
            PlayDate = bookingEntity.BookingDate,
            TotalPrice = totalPrice,
            TenantCode = currentTenantService.Code.ToString()
        };

        tenantContext.BookingHistories.Add(bookingHistoryEntities);
        await tenantContext.SaveChangesAsync(cancellationToken);

        return totalPrice;
    }

}
