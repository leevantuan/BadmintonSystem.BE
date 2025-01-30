using MassTransit;
using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Message;

[ExcludeFromTopology]
public interface IBusMessage : IRequest
{
    public Guid Id { get; set; }

    public DateTime TimeSpan { get; set; }
}
