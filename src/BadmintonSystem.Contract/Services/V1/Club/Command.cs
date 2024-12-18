using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.Club;

public static class Command
{
    public record CreateClubCommand(Guid UserId, Request.CreateClubDetailsRequest Data)
        : ICommand<Response.ClubResponse>;

    public record UpdateClubCommand(Request.UpdateClubDetailsRequest Data)
        : ICommand<Response.ClubResponse>;

    public record DeleteClubsCommand(List<string> Ids)
        : ICommand;
}
