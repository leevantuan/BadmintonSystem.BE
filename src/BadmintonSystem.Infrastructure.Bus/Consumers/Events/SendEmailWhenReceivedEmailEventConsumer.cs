using BadmintonSystem.Contract.Abstractions.IntegrationEvents;
using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Infrastructure.Bus.Consumers.Events;

public sealed class SendEmailWhenReceivedEmailEventConsumer : ConsumerEvent<BusEvent.EmailCreatedBusEvent>
{
    protected override Task Handle(BusEvent.EmailCreatedBusEvent message)
    {
        throw new NotImplementedException();
    }
}
