using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.ChatRoom;

public static class Command
{
    public record CreateChatRoomCommand(Guid UserId)
        : ICommand;
}
