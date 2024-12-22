using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Domain.Entities;

public class ChatMessage : EntityAuditBase<Guid>
{
    public string? ImageUrl { get; set; }

    public string? Content { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsRead { get; set; }

    public DateTime? ReadDate { get; set; }

    public Guid ChatRoomId { get; set; }
}
