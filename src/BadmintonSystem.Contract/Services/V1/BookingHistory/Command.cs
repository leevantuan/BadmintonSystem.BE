using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.BookingHistory;

public static class Command
{
    public record CreateBookingHistoryCommand(Request.CreateBookingHistoryRequest Data)
        : ICommand;

    public record DeleteBookingHistoryCommand(Guid BookingHistoryId)
    : ICommand;
}
