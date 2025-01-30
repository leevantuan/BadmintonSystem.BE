using MassTransit;

namespace BadmintonSystem.Contract.Abstractions.Message;

[ExcludeFromTopology]
public interface IBusCommand : IBusMessage
{
    public string Name { get; set; }

    public string Description { get; set; }

    public Guid TransactionId { get; set; }
}
