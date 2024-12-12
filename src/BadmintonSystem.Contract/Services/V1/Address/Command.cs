using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Address;

public static class Command
{
    public record CreateAddressCommand(Guid UserId, Request.CreateAddressRequest Data)
        : ICommand<Response.AddressResponse>;

    public record UpdateAddressCommand(Request.UpdateAddressRequest Data)
        : ICommand<Response.AddressResponse>;

    public record DeleteAddressesCommand(List<string> Ids)
        : ICommand;
}
