using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.BookingHistory;
using BadmintonSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.BookingHistory;

public sealed class DeleteBookingHistoryCommandHandler(
    TenantDbContext context)
    : ICommandHandler<Command.DeleteBookingHistoryCommand>
{
    public async Task<Result> Handle(Command.DeleteBookingHistoryCommand request, CancellationToken cancellationToken)
    {
        var bookingHistory = await context.BookingHistories.Where(x => x.Id == request.BookingHistoryId).FirstOrDefaultAsync(cancellationToken)
            ?? throw new ApplicationException("Booking history not found");

        context.BookingHistories.Remove(bookingHistory);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
