using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.ClubAddress;

public static class Command
{
    public record CreateClubAddressCommand(Guid UserId, Request.CreateClubAddressRequest Data)
        : ICommand;

    public record UpdateClubAddressCommand(Request.UpdateClubAddressRequest Data)
        : ICommand;

    public record DeleteClubAddressesCommand(List<Request.DeleteClubAddressRequest> Data)
        : ICommand;
}
