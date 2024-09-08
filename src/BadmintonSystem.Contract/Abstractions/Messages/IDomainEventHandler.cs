using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Messages;
public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}
