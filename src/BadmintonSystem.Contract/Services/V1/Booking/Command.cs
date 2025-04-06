using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class Command
{
    public record CreateBookingRabbitMQCommand(Guid UserId, Request.CreateBookingRequest Data)
        : ICommand;

    public record CreateBookingByChatCommand(Request.CreateBookingByChatRequest Data)
        : ICommand;

    public record CreateUrlBookingByChatCommand(Request.CreateUrlBookingByChatRequest Data)
        : ICommand<string>;

    public record CheckUnBookedByChatCommand(Request.CheckUnBookedByChatRequest Data)
        : ICommand;

    public record CreateBookingCommand(Guid UserId, Request.CreateBookingRequest Data)
        : ICommand;

    public record UpdateBookingCommand(Guid BookingId)
        : ICommand;

    public record ReserveBookingByIdCommand(Guid Id, Request.ReserveBookingRequest Data)
        : ICommand;

    public record DeleteBookingByIdCommand(Guid Id)
        : ICommand;
}
