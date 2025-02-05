using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Domain.Exceptions;
using BadmintonSystem.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace BadmintonSystem.Application.UseCases.V1.Commands.Booking;

public sealed class CreateBookingRabbitMQCommandHandler(
    ApplicationDbContext context,
    IBus bus)
    : ICommandHandler<Command.CreateBookingRabbitMQCommand>
{
    public async Task<Result> Handle(Command.CreateBookingRabbitMQCommand request, CancellationToken cancellationToken)
    {
        _ = await context.AppUsers
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken)
            ?? throw new IdentityException.AppUserNotFoundException(request.UserId);

        // Command Events
        var sendEmail = new BusCommand.SendCreateBookingCommand
        {
            Id = Guid.NewGuid(),
            Description = "Booking Description",
            Name = "Booking Notification",
            TimeSpan = DateTime.Now,
            TransactionId = Guid.NewGuid(),
            UserId = request.UserId,
            CreateBooking = request.Data,
            Type = BookingEnum.BOOKED.ToString()
        };

        ISendEndpoint endPoint = await bus.GetSendEndpoint(new Uri("queue:send-create-booking-queue"));

        await endPoint.Send(sendEmail);

        return Result.Success();
    }
}
