using BadmintonSystem.Contract.Services.V1.ChatMessage;

namespace BadmintonSystem.Application.Abstractions;

public interface IChatHub
{
    Task SendMessageToUserAsync(string userId, Response.ChatMessageResponse message);
}
