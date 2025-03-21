using BadmintonSystem.Contract.Abstractions.Entities;

namespace BadmintonSystem.Contract.Services.V1.ChatRoom;

public static class Response
{
    public class ChatRoomResponse : EntityAuditBase<Guid>
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }
    }

    public class GetChatRoomByIdResponse : ChatRoomResponse
    {
        public ChatMessage.Response.ChatMessageResponse? ChatMessage { get; set; }
    }

    public class GetBaseChatRoomByIdSqlResponse
    {
        public Guid ChatRoom_Id { get; set; }

        public Guid ChatRoom_UserId { get; set; }

        public string ChatRoom_UserName { get; set; }

        public string ChatRoom_Email { get; set; }

        public string ChatRoom_Avatar { get; set; }

        public DateTime ChatRoom_CreatedDate { get; set; }
    }

    public class GetChatRoomByIdSqlResponse : GetBaseChatRoomByIdSqlResponse
    {
        public Guid? ChatMessage_Id { get; set; }

        public string? ChatMessage_ImageUrl { get; set; }

        public string? ChatMessage_Content { get; set; }

        public bool? ChatMessage_IsAdmin { get; set; }

        public bool? ChatMessage_IsRead { get; set; }

        public DateTime? ChatMessage_ReadDate { get; set; }

        public Guid? ChatMessage_ChatRoomId { get; set; }

        public DateTime? ChatMessage_CreatedDate { get; set; }

        //public Guid? AppUser_Id { get; set; }

        //public string? AppUser_UserName { get; set; }

        //public string? AppUser_Email { get; set; }

        //public string? AppUser_FullName { get; set; }

        //public DateTime? AppUser_DateOfBirth { get; set; }

        //public string? AppUser_AvatarUrl { get; set; }
    }
}
