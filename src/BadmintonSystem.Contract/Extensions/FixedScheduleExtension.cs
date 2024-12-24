namespace BadmintonSystem.Contract.Extensions;

public static class FixedScheduleExtension
{
    public static string GetSortFixedScheduleProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "starttime" => "StartTime",
            "endtime" => "EndTime",
            _ => "Id"
        };
    }
}
