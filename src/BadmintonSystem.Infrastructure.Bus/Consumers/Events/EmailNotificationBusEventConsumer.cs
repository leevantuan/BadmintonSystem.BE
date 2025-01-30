using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Abstractions.Message;
using MediatR;

namespace BadmintonSystem.Infrastructure.Bus.Consumers.Events;

public class EmailNotificationBusEventConsumer(ISender sender)
    : ConsumerEvent<BusEvent.EmailNotificationBusEvent>(sender)
{
}
