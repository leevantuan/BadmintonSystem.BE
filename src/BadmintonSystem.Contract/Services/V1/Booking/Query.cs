using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Booking;

public static class Query
{
    public record GetBookingsQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.BookingResponse>>;

    public record GetBookingByIdQuery(Guid Id)
        : IQuery<Response.BookingResponse>;

    public record GetBookingsWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.BookingDetailResponse>>;
}
