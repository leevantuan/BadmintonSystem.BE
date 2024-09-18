using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Category;
public static class DomainEvent
{
    // Naming = Nouns + V+ed
    public record CategoryCreated(Guid Id) : IDomainEvent;

    public record CategoryUpdated(Guid Id) : IDomainEvent;

    public record CategoryDeleted(Guid Id) : IDomainEvent;
}
