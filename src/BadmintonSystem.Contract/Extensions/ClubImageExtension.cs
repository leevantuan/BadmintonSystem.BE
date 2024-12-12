namespace BadmintonSystem.Contract.Extensions;

public static class ClubImageExtension
{
    public static string GetSortClubImageProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "imagelink" => "ImageLink",
            _ => "Id"
        };
    }
}
