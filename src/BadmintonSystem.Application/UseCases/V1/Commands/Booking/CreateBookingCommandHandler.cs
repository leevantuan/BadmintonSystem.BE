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
    : ICommandHandler<Command.CreateBookingCommand, Response.BookingResponse>
{
    public async Task<Result<Response.BookingResponse>> Handle
        (Command.CreateBookingCommand request, CancellationToken cancellationToken)
    {
        AppUser user = context.AppUsers.FirstOrDefault(x => x.Id == request.UserId)
                       ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        var bookingId = Guid.NewGuid();
        var billId = Guid.NewGuid();

        var bookingEntity = new Domain.Entities.Booking
        {
            Id = bookingId,
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

        context.Booking.Add(bookingEntity);
        await context.SaveChangesAsync(cancellationToken);

        var billEntity = new Domain.Entities.Bill
        {
            Id = billId,
            BookingId = bookingId,
            UserId = request.UserId,
            TotalPrice = 0,
            Status = BillStatusEnum.BOOKED
        };

        context.Bill.Add(billEntity);
        await context.SaveChangesAsync(cancellationToken);

        decimal originalPrice =
            await bookingLineService.CreateBookingLines(bookingId, request.Data.YardPriceIds, cancellationToken);

        decimal totalPrice = originalPrice;

        if (request.Data.SaleId != null)
        {
            Domain.Entities.Sale? percentSale = context.Sale.FirstOrDefault(s => s.Id == request.Data.SaleId)
                                                ?? throw new SaleException.SaleNotFoundException(request.Data.SaleId ??
                                                    Guid.Empty);

            totalPrice -= totalPrice * percentSale.Percent / 100;
        }

        bookingEntity.BookingTotal = totalPrice;
        bookingEntity.OriginalPrice = originalPrice;

        billEntity.TotalPrice = totalPrice;
        billEntity.TotalPayment = totalPrice - totalPrice * request.Data.PercentPrePay / 100;
        billEntity.Name = request.Data.FullName ?? user.FullName;

        Response.BookingResponse? result = mapper.Map<Response.BookingResponse>(bookingEntity);

        return Result.Success(result);
    }
}
