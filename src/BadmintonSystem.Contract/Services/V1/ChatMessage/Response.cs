using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.ChatMessage;

public static class Response
{
    public class ChatMessageResponse : EntityAuditBase<Guid>
    {
        public string? ImageUrl { get; set; }

        public string? Content { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadDate { get; set; }

        public Guid ChatRoomId { get; set; }
    }

    public class GetChatMessageByIdResponse : ChatMessageResponse
    {
        public User.Response.AppUserResponse? User { get; set; }
    }

    public class GetBaseChatMessageByIdSqlResponse
    {
        public Guid ChatMessage_Id { get; set; }

        public string? ChatMessage_ImageUrl { get; set; }

        public string? ChatMessage_Content { get; set; }

        public bool ChatMessage_IsAdmin { get; set; }

        public bool ChatMessage_IsRead { get; set; }

        public DateTime? ChatMessage_ReadDate { get; set; }

        public Guid? ChatMessage_ChatRoomId { get; set; }

        public DateTime ChatMessage_CreatedDate { get; set; }
    }

    public class GetChatMessageByIdSqlResponse : GetBaseChatMessageByIdSqlResponse
    {
        public Guid? AppUser_Id { get; set; }

        public string? AppUser_UserName { get; set; }

        public string? AppUser_Email { get; set; }

        public string? AppUser_FullName { get; set; }

        public DateTime? AppUser_DateOfBirth { get; set; }

        public string? AppUser_AvatarUrl { get; set; }
    }

    public class ChatbotResponse : EntityBase<Guid>
    {
        public string Content { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsFromUser { get; set; }

        public DateTime CreatedDate { get; set; }
    }

#pragma warning disable SA1300
    public class ChatbotServerResponse
    {
        // response from chatbot rasa
        public string recipient_id { get; set; }

        public string? text { get; set; }

        public string? image { get; set; }
    }
#pragma warning restore SA1300
}
