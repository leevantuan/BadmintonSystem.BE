using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Services.V1.Booking;
using MediatR;

namespace BadmintonSystem.Infrastructure.Bus.Services;

public class EmailService(IMediator mediator) : IEmailService
{
    public async Task SendEmailAsync(BusCommand.SendEmailBusCommand request, CancellationToken cancellationToken)
    {
        // SEND_MAIL Notification Client
        if (request.Type == NotificationType.client)
        {
            await mediator.Publish(
                new DomainEvent.BookingDone(request.BookingIds, request.Name, request.Email),
                cancellationToken);
        }

        // SEND_MAIL Notification Staff
        if (request.Type == NotificationType.staff)
        {
            await mediator.Publish(
                new DomainEvent.BookingNotificationToStaff(request.BookingIds, request.Name, request.Email),
                cancellationToken);
        }
    }
}
