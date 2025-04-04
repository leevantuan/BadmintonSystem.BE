using AutoMapper;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.BookingHistory;
using BadmintonSystem.Persistence;

namespace BadmintonSystem.Application.UseCases.V1.Commands.BookingHistory;

public sealed class CreateBookingHistoryCommandHandler(
    TenantDbContext context,
    IMapper mapper)
    : ICommandHandler<Command.CreateBookingHistoryCommand>
{
    public async Task<Result> Handle(Command.CreateBookingHistoryCommand request, CancellationToken cancellationToken)
    {
        var bookingHistory = mapper.Map<Domain.Entities.BookingHistory>(request.Data);
        bookingHistory.CreatedDate = DateTime.Now;
        bookingHistory.PaymentStatus = Domain.Enumerations.PaymentStatusEnum.Paid;

        context.BookingHistories.Add(bookingHistory);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
