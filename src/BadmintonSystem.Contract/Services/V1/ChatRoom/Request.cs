namespace BadmintonSystem.Contract.Services.V1.ChatRoom;

public static class Request
{
    public class CreateChatRoomRequest
    {
        public Guid UserId { get; set; }
    }

    public class FilterChatRoomRequest
    {
        public int AppRoleType { get; set; }
    }
}
