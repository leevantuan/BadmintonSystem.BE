using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Domain.Entities;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Response = BadmintonSystem.Contract.Services.V1.Booking.Response;

namespace BadmintonSystem.Infrastructure.Bus.Services;

public class BookingService(
    ApplicationDbContext context,
    IBus bus,
    IBookingLineService bookingLineService)
    : IBookingService
{
    public async Task CreateBookingAsync
        (BusCommand.SendCreateBookingCommand data, CancellationToken cancellationToken)
    {
        try
        {
            List<Response.GetIdsByDate> idsByDate =
                await GetIdsByDateAsync(data.CreateBooking.YardPriceIds, cancellationToken);

            var bookingIds = new List<Guid>();
            foreach (Response.GetIdsByDate idByDate in idsByDate)
            {
                var bookingEntity = new Booking
                {
                    Id = Guid.NewGuid(),
                    BookingStatus = BookingStatusEnum.Pending,
                    PaymentStatus = PaymentStatusEnum.Unpaid,
                    UserId = data.UserId,
                    BookingDate = idByDate.Date,
                    BookingTotal = 0,
                    OriginalPrice = 0,
                    PercentPrePay = data.CreateBooking.PercentPrePay,
                    SaleId = data.CreateBooking.SaleId,
                    FullName = data.CreateBooking.FullName ?? "User",
                    PhoneNumber = data.CreateBooking.PhoneNumber ?? "PhoneNumber"
                };

                var billEntity = new Bill
                {
                    Id = Guid.NewGuid(),
                    UserId = bookingEntity.UserId,
                    BookingId = bookingEntity.Id,
                    TotalPrice = 0,
                    Status = BillStatusEnum.BOOKED
                };

                await CreateBookingAsync(idByDate, bookingEntity, billEntity, cancellationToken);

                bookingIds.Add(bookingEntity.Id);
            }

            await SignalRAndUpdateCacheAsync(idsByDate, cancellationToken);
            await SendMailAsync(bookingIds, data.CreateBooking.FullName, data.CreateBooking.Email, NotificationType.client, "send-mail-client-queue", cancellationToken);
            await SendMailAsync(bookingIds, data.CreateBooking.FullName, data.CreateBooking.Email, NotificationType.staff, "send-mail-staff-queue", cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Failed to create booking: {ex.Message}");
        }
    }

    private async Task SendMailAsync
        (List<Guid> bookingIds, string fullName, string email, string type, string queue, CancellationToken cancellationToken)
    {
        var sendEmailClient = new BusCommand.SendEmailBusCommand
        {
            Id = Guid.NewGuid(),
            Description = "Email Description",
            Name = fullName,
            Email = type == NotificationType.staff ? "managersystem.net@gmail.com" : email,
            TimeSpan = DateTime.Now,
            TransactionId = Guid.NewGuid(),
            BookingIds = bookingIds,
            Type = type
        };

        ISendEndpoint endPoint = await bus.GetSendEndpoint(new Uri($"queue:{queue}"));
        await endPoint.Send(sendEmailClient, cancellationToken);
    }

    private async Task SignalRAndUpdateCacheAsync
        (List<Response.GetIdsByDate> idsByDate, CancellationToken cancellationToken)
    {
        var sendSignalRAndUpdateCache = new BusCommand.SendUpdateCacheBusCommand
        {
            Id = Guid.NewGuid(),
            Description = "Email Description",
            Name = "Email Notification",
            TimeSpan = DateTime.Now,
            TransactionId = Guid.NewGuid(),
            YardPriceIds = idsByDate,
            Type = BookingEnum.BOOKED.ToString()
        };

        ISendEndpoint endPoint = await bus.GetSendEndpoint(new Uri("queue:send-update-cache-queue"));
        await endPoint.Send(sendSignalRAndUpdateCache, cancellationToken);
    }

    private async Task<List<Response.GetIdsByDate>> GetIdsByDateAsync
        (List<Guid> ids, CancellationToken cancellationToken)
    {
        List<Response.GetIdsByDate> yardPrice = await context.YardPrice
            .Where(x => ids.Contains(x.Id))
            .GroupBy(x => x.EffectiveDate.Date)
            .Select(x => new Response.GetIdsByDate
            {
                Date = x.Key,
                Ids = x.Select(y => y.Id).ToList()
            }).ToListAsync(cancellationToken);

        return yardPrice;
    }

    private async Task CreateBookingAsync
    (Response.GetIdsByDate yardPriceIds, Booking bookingEntity, Bill billEntity,
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
            Sale percentSale =
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
    }
}
