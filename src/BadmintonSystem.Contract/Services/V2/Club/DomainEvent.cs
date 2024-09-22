using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Club;
public static class DomainEvent
{
    // Naming = Nouns + V+ed
    public record ClubCreated(Guid Id) : IDomainEvent;

    public record ClubUpdated(Guid Id) : IDomainEvent;

    public record ClubDeleted(Guid Id) : IDomainEvent;
}
