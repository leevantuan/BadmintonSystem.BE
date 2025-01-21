namespace BadmintonSystem.Domain.Exceptions;

public static class ChatMessageException
{
    public class ChatMessageNotFoundException : NotFoundException
    {
        public ChatMessageNotFoundException(Guid chatMessageId)
            : base($"The chat message with the id {chatMessageId} was not found")
        {
        }

        public ChatMessageNotFoundException(Guid userId, bool isUserId = true)
            : base(isUserId
                ? $"The chat message with the user id {userId} was not found"
                : $"The chat message with the user id {userId} was not found")
        {
        }
    }

    public class ChatMessageAlreadyExistException : AlreadyExistException
    {
        public ChatMessageAlreadyExistException(Guid userId)
            : base($"The chat message with the user id {userId} already exists.")
        {
        }
    }

    public class ChatBotBadRequestException : BadRequestException
    {
        public ChatBotBadRequestException(string error)
            : base(error)
        {
        }
    }
}
