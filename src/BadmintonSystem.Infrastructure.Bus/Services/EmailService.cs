using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Enumerations;
using BadmintonSystem.Contract.Services.V1.Booking;
using MediatR;

namespace BadmintonSystem.Infrastructure.Bus.Services;

public class EmailService(IMediator mediator) : IEmailService
{
    public async Task SendEmailAsync(BusCommand.SendEmailBusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Type == NotificationType.client)
            {
                await mediator.Publish(
                    new DomainEvent.BookingDone(request.BookingIds, request.Name, request.Email),
                    cancellationToken);
            }

            if (request.Type == NotificationType.staff)
            {
                await mediator.Publish(
                    new DomainEvent.BookingNotificationToStaff(request.BookingIds, request.Name, request.Email),
                    cancellationToken);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred sending an email: {ex.Message}");
        }
    }
}
