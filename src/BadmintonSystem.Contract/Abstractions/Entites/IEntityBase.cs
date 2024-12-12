namespace BadmintonSystem.Contract.Abstractions.Entities;

public interface IEntityBase<TKey>
{
    TKey Id { get; set; }
}
