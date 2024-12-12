namespace BadmintonSystem.Contract.Extensions;

public static class BookingTimeExtension
{
    public static string GetSortBookingTimeProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            _ => "Id"
        };
    }
}
