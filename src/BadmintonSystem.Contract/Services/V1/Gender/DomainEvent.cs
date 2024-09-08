using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V1.Gender;
public static class DomainEvent
{
    // Naming = Nouns + V+ed
    public record GenderCreated(Guid Id) : IDomainEvent;

    public record GenderUpdated(Guid Id) : IDomainEvent;

    public record GenderDeleted(Guid Id) : IDomainEvent;
}
