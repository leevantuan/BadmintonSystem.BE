using BadmintonSystem.Contract.Abstractions.Message;

namespace BadmintonSystem.Contract.Services.V1.TimeSlot;

public static class Command
{
    public record CreateTimeSlotCommand(Guid UserId, Request.CreateTimeSlotRequest Data)
        : ICommand<Response.TimeSlotResponse>;

    public record UpdateTimeSlotCommand(Request.UpdateTimeSlotRequest Data)
        : ICommand<Response.TimeSlotResponse>;

    public record DeleteTimeSlotsCommand(List<string> Ids)
        : ICommand;
}
