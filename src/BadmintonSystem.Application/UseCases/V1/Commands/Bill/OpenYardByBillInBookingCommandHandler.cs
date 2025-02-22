﻿using BadmintonSystem.Application.UseCases.V1.Services;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class OpenYardByBillInBookingCommandHandler(
    IBillService billService,
    ApplicationDbContext context,
    IRepositoryBase<Domain.Entities.Bill, Guid> billRepository)
    : ICommandHandler<Command.OpenYardByBillInBookingCommand>
{
    public async Task<Result> Handle
        (Command.OpenYardByBillInBookingCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Bill billEntities = await billRepository.FindByIdAsync(request.BillId, cancellationToken)
                                            ?? throw new BillException.BillNotFoundException(request.BillId);

        if (billEntities.Status is BillStatusEnum.ACTIVE_NOW or BillStatusEnum.CLOSE_BILL)
        {
            throw new ApplicationException("You cannot open yard because it is not booking.");
        }

        await billService.ChangeYardActiveByBookingId(request.BillId, StatusEnum.FALSE, cancellationToken);

        Domain.Entities.Booking? bookingEntities =
            await context.Booking.FirstOrDefaultAsync(x => x.Id == billEntities.BookingId, cancellationToken);

        bookingEntities.BookingStatus = BookingStatusEnum.Used;
        billEntities.Status = BillStatusEnum.ACTIVE_NOW;

        return Result.Success();
    }
}
