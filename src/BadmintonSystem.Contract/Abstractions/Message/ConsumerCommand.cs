using MassTransit;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Message;

public abstract class ConsumerCommand<TMessage>(ISender sender)
    : IConsumer<TMessage>
    where TMessage : class, IBusCommand
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await sender.Send(context.Message);
    }
}
