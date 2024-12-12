namespace BadmintonSystem.Contract.Extensions;

public static class ReviewImageExtension
{
    public static string GetSortReviewImageProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "imagelink" => "ImageLink",
            _ => "Id"
        };
    }
}
