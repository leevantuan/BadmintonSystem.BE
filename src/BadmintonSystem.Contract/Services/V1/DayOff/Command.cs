using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.DayOff;

public static class Command
{
    public record CreateDayOffCommand(Guid UserId, Request.CreateDayOffRequest Data)
        : ICommand<Response.DayOffResponse>;

    public record UpdateDayOffCommand(Request.UpdateDayOffRequest Data)
        : ICommand<Response.DayOffResponse>;

    public record DeleteDayOffsCommand(List<string> Ids)
        : ICommand;
}
