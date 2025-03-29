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
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class CreateBookingRabbitMQCommandHandler(
    ApplicationDbContext context,
    IBus bus,
    IMediator mediator,
    IRedisService redisService,
    ICurrentTenantService currentTenantService,
    IBookingLineService bookingLineService)
    : ICommandHandler<Command.CreateBookingRabbitMQCommand>
{
    public async Task<Result> Handle(Command.CreateBookingRabbitMQCommand request, CancellationToken cancellationToken)
    {
        List<Contract.Services.V1.Booking.Response.GetIdsByDate> idsByDate =
                await GetIdsByDateAsync(request.Data.YardPriceIds, cancellationToken);

        var bookingIds = new List<Guid>();
        foreach (Contract.Services.V1.Booking.Response.GetIdsByDate idByDate in idsByDate)
        {
            var bookingEntity = new Domain.Entities.Booking
            {
                Id = Guid.NewGuid(),
                BookingStatus = BookingStatusEnum.Pending,
                PaymentStatus = PaymentStatusEnum.Unpaid,
                UserId = Guid.NewGuid(),
                BookingDate = idByDate.Date,
                BookingTotal = 0,
                OriginalPrice = 0,
                PercentPrePay = request.Data.PercentPrePay,
                SaleId = request.Data.SaleId,
                FullName = request.Data.FullName ?? "User",
                PhoneNumber = request.Data.PhoneNumber ?? "PhoneNumber"
            };

            var billEntity = new Domain.Entities.Bill
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

        await SignalRAndUpdateCacheAsync(idsByDate, request.Data.Tenant, cancellationToken);
        await SendMailAsync(bookingIds, request.Data.FullName, request.Data.Email, NotificationType.client, cancellationToken);
        await SendMailAsync(bookingIds, request.Data.FullName, request.Data.Email, NotificationType.staff, cancellationToken);

        string endpoint = $"BMTSYS_{currentTenantService.Code.ToString()}-get-yard-prices-by-date";

        await redisService.DeletesAsync(endpoint);

        return Result.Success();
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

        ISendEndpoint endPoint = await bus.GetSendEndpoint(new Uri("queue:send-update-cache-queue"));
        await endPoint.Send(sendSignalRAndUpdateCache, cancellationToken);
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

    private async Task CreateBookingAsync
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
    }
}
