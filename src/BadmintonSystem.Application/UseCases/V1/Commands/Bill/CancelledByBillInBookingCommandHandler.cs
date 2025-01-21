using BadmintonSystem.Application.Abstractions;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
using BadmintonSystem.Domain.Abstractions.Repositories;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Bill;

public sealed class CancelledByBillInBookingCommandHandler(
    ApplicationDbContext context,
    IBookingHub bookingHub,
    IRepositoryBase<Domain.Entities.Bill, Guid> billRepository)
    : ICommandHandler<Command.CancelledByBillInBookingCommand>
{
    public async Task<Result> Handle
        (Command.CancelledByBillInBookingCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Bill billEntities = await billRepository.FindByIdAsync(request.BillId, cancellationToken)
                                            ?? throw new BillException.BillNotFoundException(request.BillId);

        if (billEntities.Status is BillStatusEnum.ACTIVE_NOW or BillStatusEnum.CLOSE_BILL)
        {
            throw new ApplicationException("You cannot open yard because it is not booking.");
        }

        Domain.Entities.Booking? bookingEntities =
            await context.Booking.FirstOrDefaultAsync(x => x.Id == billEntities.BookingId, cancellationToken);

        bookingEntities.BookingStatus = BookingStatusEnum.Cancelled;
        billEntities.Status = BillStatusEnum.CLOSE_BILL;

        // await bookingHub.SendBookingMessageToUserAsync("f443be1e-0642-4ff7-b3cc-183ef6548494",
        //     new Response.BookingHubResponse
        //     {
        //         Id = request.BillId,
        //         Type = BillStatusEnum.CLOSE_BILL.ToString()
        //     });

        return Result.Success();
    }
}
