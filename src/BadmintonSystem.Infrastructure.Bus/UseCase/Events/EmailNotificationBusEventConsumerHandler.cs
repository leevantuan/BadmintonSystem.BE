using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using MediatR;

namespace BadmintonSystem.Infrastructure.Bus.UseCase.Events;

public sealed class EmailNotificationBusEventConsumerHandler : IRequestHandler<BusEvent.EmailNotificationBusEvent>
{
    public Task Handle(BusEvent.EmailNotificationBusEvent request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
