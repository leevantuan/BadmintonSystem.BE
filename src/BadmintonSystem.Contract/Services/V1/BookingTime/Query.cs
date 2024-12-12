using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.BookingTime;

public static class Query
{
    public record GetBookingTimeesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.BookingTimeResponse>>;

    public record GetBookingTimeByIdQuery(Guid Id)
        : IQuery<Response.BookingTimeResponse>;
}
