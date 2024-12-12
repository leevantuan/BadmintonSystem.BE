using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Message;
public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
