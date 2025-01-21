using BadmintonSystem.Contract.Abstractions.Entities;
using BadmintonSystem.Domain.Entities.Identity;

namespace BadmintonSystem.Domain.Entities;

public class ChatRoom : EntityAuditBase<Guid>
{
    public Guid UserId { get; set; }

    public virtual AppUser? User { get; set; }

    public virtual ICollection<ChatMessage>? ChatMessages { get; set; }
}
