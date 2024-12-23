namespace BadmintonSystem.Contract.Extensions;

public static class YardPriceExtension
{
    public static string GetSortYardPriceProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "yardid" => "YardId",
            "priceid" => "PriceId",
            "timeslotid" => "TimeSlotId",
            "effectivedate" => "EffectiveDate",
            "isbooking" => "IsBooking",
            _ => "Id"
        };
    }
}
