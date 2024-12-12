namespace BadmintonSystem.Contract.Extensions;

public static class TimeSlotExtension
{
    public static string GetSortTimeSlotProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "starttime" => "StartTime",
            "endtime" => "EndTime",
            _ => "Id"
        };
    }
}
