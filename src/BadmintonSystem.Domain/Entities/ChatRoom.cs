using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class ChatRoom : EntityAuditBase<Guid>
{
    public Guid UserId { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string Avatar { get; set; }

    //public virtual AppUser? User { get; set; }

    public virtual ICollection<ChatMessage>? ChatMessages { get; set; }
}
