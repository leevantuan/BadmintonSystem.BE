namespace BadmintonSystem.Contract.Abstractions.Entities;

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}
