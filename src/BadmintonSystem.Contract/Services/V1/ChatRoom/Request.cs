namespace BadmintonSystem.Contract.Services.V1.ChatRoom;

public static class Request
{
    public class CreateChatRoomRequest
    {
        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }
    }

    public class FilterChatRoomRequest
    {
        public int AppRoleType { get; set; }

        public Guid? UserId { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? Avatar { get; set; }
    }
}
