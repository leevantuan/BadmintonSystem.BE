namespace BadmintonSystem.Contract.Extensions;

public static class DayOffExtension
{
    public static string GetSortDayOffProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "date" => "Date",
            "content" => "Content",
            _ => "Id"
        };
    }
}
