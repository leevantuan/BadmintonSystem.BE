using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.ChatMessage;

public static class Command
{
    public record CreateChatMessageCommand(bool IsAdmin, Request.CreateChatMessageRequest Data)
        : ICommand<Response.ChatMessageResponse>;

    public record CreateChatMessageByChatbotCommand(Request.CreateChatMessageByChatbotRequest Data)
        : ICommand<Response.ChatbotResponse>;

    public record ReadAllByUserIdCommand(Guid ChatRoomId, Guid UserId)
        : ICommand;
}
