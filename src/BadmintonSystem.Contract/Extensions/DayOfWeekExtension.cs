namespace BadmintonSystem.Contract.Extensions;

public static class DayOfWeekExtension
{
    public static string GetSortDayOfWeekProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "weekname" => "WeekName",
            _ => "Id"
        };
    }
}
