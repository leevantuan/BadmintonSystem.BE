using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.BookingTime;

public static class Command
{
    public record CreateBookingTimeCommand(Guid UserId, Request.CreateBookingTimeRequest Data)
        : ICommand;

    public record UpdateBookingTimeCommand(Request.UpdateBookingTimeRequest Data)
        : ICommand;

    public record DeleteBookingTimeesCommand(List<Request.DeleteBookingTimeRequest> Data)
        : ICommand;
}
