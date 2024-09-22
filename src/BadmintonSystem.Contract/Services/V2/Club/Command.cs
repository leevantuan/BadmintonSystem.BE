using BadmintonSystem.Contract.Abstractions.Messages;

namespace BadmintonSystem.Contract.Services.V2.Club;
public static class Command
{
    public record CreateClubCommand(Request.ClubRequest Data) : ICommand;
    public record UpdateClubCommand(Guid Id, Request.ClubRequest Data) : ICommand;
    public record DeleteClubCommand(Guid Id) : ICommand;
}
