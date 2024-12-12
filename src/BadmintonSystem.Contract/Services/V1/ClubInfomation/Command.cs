using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.ClubInformation;

public static class Command
{
    public record CreateClubInformationCommand(Guid UserId, Request.CreateClubInformationRequest Data)
        : ICommand;

    public record UpdateClubInformationCommand(Request.UpdateClubInformationRequest Data)
        : ICommand;

    public record DeleteClubInformationsCommand(List<string> Ids)
        : ICommand;
}
