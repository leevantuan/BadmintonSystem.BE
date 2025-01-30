using MassTransit;

namespace BadmintonSystem.Contract.Abstractions.Message;

[ExcludeFromTopology]
public interface IBusMessage
{
    public Guid Id { get; set; }

    public DateTime TimeSpan { get; set; }
}
