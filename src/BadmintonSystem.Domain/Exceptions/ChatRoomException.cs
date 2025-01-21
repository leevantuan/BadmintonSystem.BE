namespace BadmintonSystem.Domain.Exceptions;

public static class ChatRoomException
{
    public class ChatRoomNotFoundException : NotFoundException
    {
        public ChatRoomNotFoundException(Guid chatRoomId)
            : base($"The chat room with the id {chatRoomId} was not found")
        {
        }

        public ChatRoomNotFoundException(Guid userId, bool isUserId = true)
            : base(isUserId
                ? $"The chat room with the user id {userId} was not found"
                : $"The chat room with the user id {userId} was not found")
        {
        }
    }

    public class ChatRoomAlreadyExistException : AlreadyExistException
    {
        public ChatRoomAlreadyExistException(Guid userId)
            : base($"The chat room with the user id {userId} already exists.")
        {
        }
    }
}
