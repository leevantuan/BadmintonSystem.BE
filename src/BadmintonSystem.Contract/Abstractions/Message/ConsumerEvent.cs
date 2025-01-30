using MassTransit;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Message;

public abstract class ConsumerEvent<TMessage>(ISender sender)
    : IConsumer<TMessage>
    where TMessage : class, IBusEvent
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await sender.Send(context.Message);
    }
}
