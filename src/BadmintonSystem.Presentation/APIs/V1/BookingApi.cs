using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Booking;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Persistence.Helpers;
using BadmintonSystem.Presentation.Abstractions;
using BadmintonSystem.Presentation.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Request = BadmintonSystem.Contract.Services.V1.Booking.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class BookingApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/bookings";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("bookings")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateBookingV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("filter-and-sort-value", GetBookingsFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.READ);

        group1.MapPost("filter-and-sort-value-by-date", GetBookingsFilterAndSortValueByDateV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{bookingId}", GetBookingByIdV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{bookingId}", UpdateBookingV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteBookingsV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateBookingV1
    (
        ISender sender,
        [FromBody] Request.CreateBookingRequest createBooking,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.BookingResponse> result =
            await sender.Send(new Command.CreateBookingCommand(userId ?? Guid.Empty, createBooking));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteBookingsV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteBookingsCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateBookingV1
    (
        ISender sender,
        Guid id
    )
    {
        Result result = await sender.Send(new Command.UpdateBookingCommand(id));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetBookingByIdV1(ISender sender, Guid bookingId)
    {
        Result<Response.BookingResponse> result = await sender.Send(new Query.GetBookingByIdQuery(bookingId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetBookingsFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.BookingDetail>> result =
            await sender.Send(new Query.GetBookingsWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }

    private static async Task<IResult> GetBookingsFilterAndSortValueByDateV1
    (
        ISender sender,
        [FromBody] DateTime date,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.GetBookingDetailResponse>> result =
            await sender.Send(new Query.GetBookingsByDateFilterAndSortValueQuery(date, pagedQueryRequest));

        return Results.Ok(result);
    }
}
