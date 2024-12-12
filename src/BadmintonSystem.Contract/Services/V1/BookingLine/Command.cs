using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.BookingLine;

public static class Command
{
    public record CreateBookingLineCommand(Guid UserId, Request.CreateBookingLineRequest Data)
        : ICommand<Response.BookingLineResponse>;

    public record UpdateBookingLineCommand(Request.UpdateBookingLineRequest Data)
        : ICommand<Response.BookingLineResponse>;

    public record DeleteBookingLinesCommand(List<string> Ids)
        : ICommand;
}
