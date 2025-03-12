using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Application.Extensions;

public static class TimeSlotEnumExtension
{
    private static string GetTimeSlot(this TimeSlotEnum slot)
    {
        return slot switch
        {
            TimeSlotEnum.TimeSlot1 => "04:35:00 - 05:35:00",
            TimeSlotEnum.TimeSlot2 => "05:40:00 - 06:40:00",
            TimeSlotEnum.TimeSlot3 => "06:45:00 - 07:45:00",
            TimeSlotEnum.TimeSlot4 => "08:00:00 - 09:00:00",
            TimeSlotEnum.TimeSlot5 => "09:05:00 - 10:05:00",
            TimeSlotEnum.TimeSlot6 => "10:10:00 - 11:10:00",
            TimeSlotEnum.TimeSlot7 => "14:30:00 - 15:30:00",
            TimeSlotEnum.TimeSlot8 => "15:35:00 - 16:35:00",
            TimeSlotEnum.TimeSlot9 => "16:40:00 - 17:40:00",
            TimeSlotEnum.TimeSlot10 => "17:45:00 - 18:45:00",
            TimeSlotEnum.TimeSlot11 => "18:50:00 - 19:50:00",
            TimeSlotEnum.TimeSlot12 => "19:55:00 - 20:55:00",
            TimeSlotEnum.TimeSlot13 => "21:00:00 - 22:00:00",
            _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, null)
        };
    }

    public static (TimeSpan StartTime, TimeSpan EndTime) GetTimeSlotTimes(TimeSlotEnum slot)
    {
        string timeSlotString = GetTimeSlot(slot);
        string[] times = timeSlotString.Split(" - ");

        var startTime = TimeSpan.Parse(times[0]);
        var endTime = TimeSpan.Parse(times[1]);

        return (startTime, endTime);
    }
}
