namespace BadmintonSystem.Contract.Services.V1.ChatMessage;

public static class Request
{
    public class CreateChatMessageRequest
    {
        public string? ImageUrl { get; set; }

        public string? Content { get; set; }

        public Guid? UserId { get; set; }

        public bool IsAdmin { get; set; }
    }

    public class CreateChatMessageByChatbotRequest
    {
        public string Content { get; set; }
    }
}
