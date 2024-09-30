using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Sale;
public static class DomainEvent
{
    // Naming = Nouns + V+ed
    public record SaleCreated(Guid Id) : IDomainEvent;

    public record SaleUpdated(Guid Id) : IDomainEvent;

    public record SaleDeleted(Guid Id) : IDomainEvent;
}
