namespace BadmintonSystem.Contract.Extensions;
public static class NotificationExtension
{
    public static string GetSortNotificationProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "content" => "Content",
            _ => "Id"
        };
}
