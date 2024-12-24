using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.FixedSchedule;

public static class Query
{
    public record GetFixedScheduleByIdQuery(Guid Id)
        : IQuery<Response.FixedScheduleDetailResponse>;

    public record GetFixedSchedulesWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.FixedScheduleDetailResponse>>;
}
