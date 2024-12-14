using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.User;

public static class Command
{
    public record CreateAddressByUserIdCommand(Guid UserId, Address.Request.CreateAddressRequest Data) : ICommand;
}
