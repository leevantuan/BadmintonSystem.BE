namespace BadmintonSystem.Contract.Extensions;

public static class BookingExtension
{
    public static string GetSortBookingProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "bookingtotal" => "BookingTotal",
            "bookingdate" => "BookingDate",
            _ => "Id"
        };
    }
}
