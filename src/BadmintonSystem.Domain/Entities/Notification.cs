using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Domain.Entities;

public class Notification : EntityAuditBase<Guid>
{
    public string Content { get; set; }

    public IsReadEnum IsRead { get; set; }

    public Guid UserId { get; set; }
}
