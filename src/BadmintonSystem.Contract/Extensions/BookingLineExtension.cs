namespace BadmintonSystem.Contract.Extensions;

public static class BookingLineExtension
{
    public static string GetSortBookingLineProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "totalprice" => "TotalPrice",
            _ => "Id"
        };
    }
}
