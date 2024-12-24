using BadmintonSystem.Domain.Enumerations;

namespace BadmintonSystem.Application.Extensions;

public static class TimeSlotEnumExtension
{
    public static string GetTimeSlot(this TimeSlotEnum slot)
    {
        return slot switch
        {
            TimeSlotEnum.TimeSlot1 => "00:15:00 - 01:15:00",
            TimeSlotEnum.TimeSlot2 => "01:20:00 - 02:20:00",
            TimeSlotEnum.TimeSlot3 => "02:25:00 - 03:25:00",
            TimeSlotEnum.TimeSlot4 => "03:30:00 - 04:30:00",
            TimeSlotEnum.TimeSlot5 => "04:35:00 - 05:35:00",
            TimeSlotEnum.TimeSlot6 => "05:40:00 - 06:40:00",
            TimeSlotEnum.TimeSlot7 => "06:45:00 - 07:45:00",

            TimeSlotEnum.TimeSlot8 => "08:00:00 - 09:00:00",
            TimeSlotEnum.TimeSlot9 => "09:05:00 - 10:05:00",
            TimeSlotEnum.TimeSlot10 => "10:10:00 - 11:10:00",
            TimeSlotEnum.TimeSlot11 => "11:15:00 - 12:15:00",
            TimeSlotEnum.TimeSlot12 => "12:20:00 - 13:20:00",
            TimeSlotEnum.TimeSlot13 => "13:25:00 - 14:25:00",
            TimeSlotEnum.TimeSlot14 => "14:30:00 - 15:30:00",
            TimeSlotEnum.TimeSlot15 => "15:35:00 - 16:35:00",
            TimeSlotEnum.TimeSlot16 => "16:40:00 - 17:40:00",
            TimeSlotEnum.TimeSlot17 => "17:45:00 - 18:45:00",
            TimeSlotEnum.TimeSlot18 => "18:50:00 - 19:50:00",
            TimeSlotEnum.TimeSlot19 => "19:55:00 - 20:55:00",
            TimeSlotEnum.TimeSlot20 => "21:00:00 - 22:00:00",
            TimeSlotEnum.TimeSlot21 => "22:05:00 - 23:05:00",
            TimeSlotEnum.TimeSlot22 => "23:10:00 - 00:10:00",
            _ => throw new ArgumentOutOfRangeException(nameof(slot), slot, null)
        };
    }
}
