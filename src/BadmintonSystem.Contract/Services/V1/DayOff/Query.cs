using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.DayOff;

public static class Query
{
    public record GetDayOffByIdQuery(Guid Id)
        : IQuery<Response.DayOffDetailResponse>;

    public record GetDayOffsWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.DayOffDetailResponse>>;

    public record GetDayOffByDate(DateTime Date)
        : IQuery<Response.DayOffDetailResponse>;
}
