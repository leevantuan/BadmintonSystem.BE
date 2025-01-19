using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class CreateBookingCommandHandler(
    ApplicationDbContext context,
    IBookingLineService bookingLineService)
    : ICommandHandler<Command.CreateBookingCommand>
{
    public async Task<Result> Handle(Command.CreateBookingCommand request, CancellationToken cancellationToken)
    {
        AppUser user = await context.AppUsers
                           .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
                       ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        List<Response.GetIdsByDate> idsByDate = await GetIdsByDateAsync(request.Data.YardPriceIds, cancellationToken);

        foreach (Response.GetIdsByDate idByDate in idsByDate)
        {
            var bookingEntity = new Domain.Entities.Booking
            {
                Id = Guid.NewGuid(),
                BookingStatus = BookingStatusEnum.Pending,
                PaymentStatus = PaymentStatusEnum.Unpaid,
                UserId = request.UserId,
                BookingDate = idByDate.Date,
                BookingTotal = 0,
                OriginalPrice = 0,
                PercentPrePay = request.Data.PercentPrePay,
                SaleId = request.Data.SaleId,
                FullName = request.Data.FullName ?? user.FullName,
                PhoneNumber = request.Data.PhoneNumber ?? user.PhoneNumber
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
        }

        return Result.Success();
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
    (Response.GetIdsByDate yardPriceIds, Domain.Entities.Booking bookingEntity, Domain.Entities.Bill billEntity,
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
