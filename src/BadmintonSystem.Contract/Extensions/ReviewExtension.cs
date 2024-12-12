namespace BadmintonSystem.Contract.Extensions;

public static class ReviewExtension
{
    public static string GetSortReviewProperty(string sortColumn)
    {
        return sortColumn.ToLower() switch
        {
            "comment" => "Comment",
            "ratingvalue" => "RatingValue",
            _ => "Id"
        };
    }
}
