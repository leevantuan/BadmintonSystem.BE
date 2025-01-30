using MassTransit;

namespace BadmintonSystem.Contract.Abstractions.Message;

public abstract class ConsumerEvent<TMessage> : IConsumer<TMessage>
    where TMessage : class, IBusEvent
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await Handle(context.Message);
    }

    protected abstract Task Handle(TMessage message);
}
