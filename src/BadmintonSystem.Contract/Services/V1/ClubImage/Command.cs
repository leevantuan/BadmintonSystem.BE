using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.ClubImage;

public static class Command
{
    public record CreateClubImageCommand(Guid UserId, Request.CreateClubImageRequest Data)
        : ICommand;

    public record UpdateClubImageCommand(Request.UpdateClubImageRequest Data)
        : ICommand;

    public record DeleteClubImagesCommand(List<string> Ids)
        : ICommand;
}
