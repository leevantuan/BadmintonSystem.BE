using MassTransit;

namespace BadmintonSystem.Contract.Abstractions.Message;

public abstract class ConsumerCommand<TMessage> : IConsumer<TMessage>
    where TMessage : class, IBusCommand
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        throw new NotImplementedException();
    }
}
