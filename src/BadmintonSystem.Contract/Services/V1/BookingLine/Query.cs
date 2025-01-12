using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.BookingLine;

public static class Query
{
    public record GetBookingLinesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.BookingLineResponse>>;

    public record GetBookingLineByIdQuery(Guid Id)
        : IQuery<Response.BookingLineResponse>;
}
