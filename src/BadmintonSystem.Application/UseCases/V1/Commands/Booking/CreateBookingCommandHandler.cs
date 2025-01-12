﻿using AutoMapper;
using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Entities;
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
            PercentPrePay = request.Data.PercentPrePay,
            SaleId = request.Data.SaleId ?? null
        };

        context.Booking.Add(bookingEntity);
        await context.SaveChangesAsync(cancellationToken);

        var billEntity = new Bill
        {
            Id = billId,
            BookingId = bookingId,
            TotalPrice = 0
        };

        context.Bill.Add(billEntity);
        await context.SaveChangesAsync(cancellationToken);

        decimal totalPrice =
            await bookingLineService.CreateBookingLines(bookingId, request.Data.YardPriceIds, cancellationToken);

        if (request.Data.SaleId != null)
        {
            Domain.Entities.Sale? percentSale = context.Sale.FirstOrDefault(s => s.Id == request.Data.SaleId)
                                                ?? throw new SaleException.SaleNotFoundException(request.Data.SaleId ??
                                                    Guid.Empty);

            totalPrice -= totalPrice * percentSale.Percent / 100;
        }

        bookingEntity.BookingTotal = totalPrice;

        Response.BookingResponse? result = mapper.Map<Response.BookingResponse>(bookingEntity);

        return Result.Success(result);
    }
}
