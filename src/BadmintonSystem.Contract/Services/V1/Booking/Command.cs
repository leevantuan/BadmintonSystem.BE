using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class Command
{
    public record CreateBookingCommand(Guid UserId, Request.CreateBookingRequest Data)
        : ICommand<Response.BookingResponse>;

    public record UpdateBookingCommand(Guid BookingId)
        : ICommand;

    public record DeleteBookingsCommand(List<string> Ids)
        : ICommand;
}
