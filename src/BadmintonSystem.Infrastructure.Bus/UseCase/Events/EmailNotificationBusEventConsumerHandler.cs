using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BadmintonSystem.Infrastructure.Bus.UseCase.Events;

public sealed class EmailNotificationBusEventConsumerHandler(
    ILogger<EmailNotificationBusEventConsumerHandler> logger)
    : IRequestHandler<BusEvent.EmailNotificationBusEvent>
{
    public async Task Handle(BusEvent.EmailNotificationBusEvent request, CancellationToken cancellationToken)
    {
        await Task.Delay(10000, cancellationToken);
        logger.LogInformation($"Email notification bus event processed: {request.Name}");
    }
}
