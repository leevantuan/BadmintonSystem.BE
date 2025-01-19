using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.DayOff;

public static class Query
{
    public record GetDayOffByDateQuery(DateTime Date)
        : IQuery<Response.DayOffDetailResponse>;
}
