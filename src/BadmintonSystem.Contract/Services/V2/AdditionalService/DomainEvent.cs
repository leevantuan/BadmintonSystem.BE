using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.AdditionalService;
public static class DomainEvent
{
    // Naming = Nouns + V+ed
    public record AdditionalServiceCreated(Guid Id) : IDomainEvent;

    public record AdditionalServiceUpdated(Guid Id) : IDomainEvent;

    public record AdditionalServiceDeleted(Guid Id) : IDomainEvent;
}
