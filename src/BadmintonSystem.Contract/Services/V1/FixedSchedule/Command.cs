using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.FixedSchedule;

public static class Command
{
    public record CreateFixedScheduleCommand(Guid UserId, Request.CreateFixedScheduleRequest Data)
        : ICommand<Response.FixedScheduleResponse>;

    public record UpdateFixedScheduleCommand(Request.UpdateFixedScheduleRequest Data)
        : ICommand<Response.FixedScheduleResponse>;

    public record DeleteFixedSchedulesCommand(List<string> Ids)
        : ICommand;
}
