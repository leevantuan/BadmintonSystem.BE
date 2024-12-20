using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Address;

public static class Command
{
    // ADDRESS BY USER
    public record CreateAddressByUserIdCommand(Guid UserId, Request.CreateAddressRequest Data) : ICommand;

    public record UpdateAddressByUserIdCommand(Guid UserId, Request.UpdateAddressByUserIdRequest Data) : ICommand;

    public record DeleteAddressByUserIdCommand(Guid UserId, Guid AddressId) : ICommand;
}
