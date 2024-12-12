namespace BadmintonSystem.Contract.Extensions;

public static class ClubInformationExtension
{
    public static string GetSortClubInformationProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "facebookpagelink" => "FacebookPageLink",
            "instagramlink" => "InstagramLink",
            "maplink" => "MapLink",
            _ => "Id"
        };
    }
}
