namespace BadmintonSystem.Domain.Abstractions.Entities;
public abstract class AuditableEntity<T>
{
    public AuditableEntity()
    {
        DateCreated = DateTime.Now;
        DateModified = DateTime.Now;
        DeleteFlag = false;
    }

    public bool IsTransient()
    {
        return Id.Equals(default(T));
    }

    public virtual T Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public Guid? UserIdCreated { get; set; }
    public Guid? UserIdModified { get; set; }
    public bool DeleteFlag { get; set; }
}
