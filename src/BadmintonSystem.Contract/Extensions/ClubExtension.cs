namespace BadmintonSystem.Contract.Extensions;
public static class ClubExtension
{
    public static string GetSortClubProperty(string sortColumn)
        => sortColumn.ToLower() switch
        {
            "name" => "Name",
            "code" => "Code",
            "hotline" => "Hotline",
            "openingtime" => "OpeningTime",
            "closingtime" => "ClosingTime",
            _ => "Id"
        };
}
