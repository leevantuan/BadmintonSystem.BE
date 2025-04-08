using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.BookingHistory;

public static class Query
{
    public record GetBookingHistoriesWithFilterAndSortValueQuery(
        Guid UserId,
        Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.BookingHistoryDetailResponse>>;

    public record GetBookingHistoriesByDateQuery(
        Guid UserId,
        DateTime Date)
        : IQuery<List<Response.BookingHistoryDetailResponse>>;
}
