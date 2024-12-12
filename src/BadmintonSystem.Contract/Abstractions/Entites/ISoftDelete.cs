namespace BadmintonSystem.Contract.Abstractions.Entities;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }

    DateTime? DeletedAt { get; set; }

    public void Undo()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
}
