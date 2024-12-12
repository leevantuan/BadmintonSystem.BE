using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.UserAddress;

public static class Command
{
    public record CreateUserAddressCommand(Guid UserId, Request.CreateUserAddressRequest Data)
        : ICommand;

    public record UpdateUserAddressCommand(Request.UpdateUserAddressRequest Data)
        : ICommand;

    public record DeleteUserAddressByIdCommand(Request.DeleteUserAddressRequest Data)
        : ICommand;

    public record DeleteUserAddressesCommand(List<Request.DeleteUserAddressRequest> Data)
        : ICommand;
}
