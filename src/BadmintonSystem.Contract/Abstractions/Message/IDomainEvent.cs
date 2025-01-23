using MediatR;

namespace BadmintonSystem.Contract.Abstractions.Message;

public interface IDomainEvent : INotification
{
    public List<Guid> Ids { get; init; }
}
