using MassTransit;

namespace BadmintonSystem.Contract.Abstractions.Message;

[ExcludeFromTopology]
public interface IBusEvent : IBusMessage
{
}
