using BadmintonSystem.Contract.Enumerations;

namespace BadmintonSystem.Contract.Extensions;
public static class SortOrderExtension
{
    // handle user doesn't enter sort column or sort order
    public static SortOrder ConvertStringToSortOrderWithOneColumn(string? sortOrder)
        => !string.IsNullOrWhiteSpace(sortOrder)
            ? sortOrder.Trim().ToUpper().Equals("ASC")
            ? SortOrder.Ascending : SortOrder.Descending : SortOrder.Descending;

    public static IDictionary<string, SortOrder> ConvertStringToSortOrderWithMultipleColumn(string? sortColumnAndOrder)
    {
        var result = new Dictionary<string, SortOrder>();

        if (!string.IsNullOrEmpty(sortColumnAndOrder))
        {
            if (sortColumnAndOrder.Trim().Split(",").Length > 0)
            {
                foreach (var item in sortColumnAndOrder.Trim().Split(","))
                {
                    if (!item.Contains("-"))
                        throw new FormatException("Sort condition should be followed by format: Column1-ASC, Column2-DESC...");

                    var property = item.Trim().Split("-");
                    //var key = ReviewExtension.GetSortReviewProperty(property[0]);
                    var value = ConvertStringToSortOrderWithOneColumn(property[1]);
                    result.TryAdd(property[0], value); // it will not add duplicate key
                }
            }
            else
            {
                if (!sortColumnAndOrder.Contains("-"))
                    throw new FormatException("Sort condition should be followed by format: Column1-ASC, Column2-DESC...");

                var property = sortColumnAndOrder.Trim().Split("-");
                result.Add(property[0], ConvertStringToSortOrderWithOneColumn(property[1]));
            }
        }

        return result;
    }
}
