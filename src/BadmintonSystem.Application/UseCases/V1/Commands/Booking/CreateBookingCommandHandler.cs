using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class CreateBookingCommandHandler(
    ApplicationDbContext context,
    IMapper mapper,
    IBookingLineService bookingLineService,
    IRepositoryBase<Domain.Entities.Booking, Guid> bookingRepository)
    : ICommandHandler<Command.CreateBookingCommand>
{
    public async Task<Result> Handle
        (Command.CreateBookingCommand request, CancellationToken cancellationToken)
    {
        AppUser user = context.AppUsers.FirstOrDefault(x => x.Id == request.UserId)
                       ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        List<Response.GetIdsByDate> idsByDate = GetIdsByDate(request.Data.YardPriceIds);

        var bookingEntities = new Domain.Entities.Booking
        {
            BookingStatus = BookingStatusEnum.Pending,
            PaymentStatus = PaymentStatusEnum.Unpaid,
            UserId = request.UserId,
            BookingDate = DateTime.Now,
            BookingTotal = 0,
            OriginalPrice = 0,
            PercentPrePay = request.Data.PercentPrePay,
            SaleId = request.Data.SaleId ?? null,
            FullName = request.Data.FullName ?? user.FullName,
            PhoneNumber = request.Data.PhoneNumber ?? user.PhoneNumber
        };

        if (idsByDate.Count > 0)
        {
            foreach (Response.GetIdsByDate idByDate in idsByDate)
            {
                await CreateBooking(idByDate, bookingEntities, cancellationToken);
            }
        }

        return Result.Success();
    }

    private List<Response.GetIdsByDate> GetIdsByDate(List<Guid> ids)
    {
        var yardPrice = context.YardPrice.Where(x => ids.Contains(x.Id)).ToList();

        var result = yardPrice.GroupBy(x => x.EffectiveDate.Date)
            .Select(x => new Response.GetIdsByDate
            {
                Date = x.Key,
                Ids = x.Select(y => y.Id).ToList()
            }).ToList();

        return result;
    }

    private async Task CreateBooking
    (Response.GetIdsByDate yardPriceIds, Domain.Entities.Booking bookingEntities,
        CancellationToken cancellationToken)
    {
        var bookingId = Guid.NewGuid();
        var billId = Guid.NewGuid();

        bookingEntities.Id = bookingId;
        bookingEntities.BookingDate = yardPriceIds.Date;

        context.Booking.Add(bookingEntities);
        await context.SaveChangesAsync(cancellationToken);

        var billEntities = new Domain.Entities.Bill
        {
            Id = billId,
            BookingId = bookingId,
            UserId = bookingEntities.UserId,
            TotalPrice = 0,
            Status = BillStatusEnum.BOOKED
        };

        context.Bill.Add(billEntities);
        await context.SaveChangesAsync(cancellationToken);

        decimal originalPrice =
            await bookingLineService.CreateBookingLines(bookingId, yardPriceIds.Ids, cancellationToken);

        decimal totalPrice = originalPrice;

        if (bookingEntities.SaleId != null)
        {
            Domain.Entities.Sale? percentSale = context.Sale.FirstOrDefault(s => s.Id == bookingEntities.SaleId)
                                                ?? throw new SaleException.SaleNotFoundException(
                                                    bookingEntities.SaleId ??
                                                    Guid.Empty);

            totalPrice -= totalPrice * percentSale.Percent / 100;
        }

        bookingEntities.BookingTotal = totalPrice;
        bookingEntities.OriginalPrice = originalPrice;

        billEntities.TotalPrice = totalPrice;
        billEntities.TotalPayment = totalPrice * bookingEntities.PercentPrePay / 100;
        billEntities.Name = bookingEntities.FullName;

        await context.SaveChangesAsync(cancellationToken);
    }
}
