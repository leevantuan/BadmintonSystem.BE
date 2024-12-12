using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;
public class Notification : EntityAuditBase<Guid>
{
    public string Content { get; set; }

    public Guid UserId { get; set; }
}
