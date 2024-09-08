using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Messages;
public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}
