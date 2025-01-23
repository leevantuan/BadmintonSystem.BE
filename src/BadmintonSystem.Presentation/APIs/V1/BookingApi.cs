﻿using BadmintonSystem.Contract.Abstractions.Shared;
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
            .RequireJwtAuthorize(FunctionEnum.BOOKING.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("filter-and-sort-value", GetBookingsFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.BOOKING.ToString(), (int)ActionEnum.READ);

        group1.MapPost("filter-and-sort-value-by-date", GetBookingsFilterAndSortValueByDateV1)
            .RequireJwtAuthorize(FunctionEnum.BOOKING.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{bookingId}", GetBookingByIdV1)
            .RequireJwtAuthorize(FunctionEnum.BOOKING.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{bookingId}", UpdateBookingV1)
            .RequireJwtAuthorize(FunctionEnum.BOOKING.ToString(), (int)ActionEnum.UPDATE);

        group1.MapPut("reserve/{bookingId}", ReserveBookingV1)
            .RequireJwtAuthorize(FunctionEnum.BOOKING.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete("{bookingId}", DeleteBookingsV1)
            .RequireJwtAuthorize(FunctionEnum.BOOKING.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateBookingV1
    (
        ISender sender,
        [FromBody] Request.CreateBookingRequest createBooking,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreateBookingCommand(userId ?? Guid.Empty, createBooking));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteBookingsV1(ISender sender, Guid bookingId)
    {
        Result result = await sender.Send(new Command.DeleteBookingByIdCommand(bookingId));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateBookingV1
    (
        ISender sender,
        Guid id
    )
    {
        Result result = await sender.Send(new Command.UpdateBookingCommand(id));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> ReserveBookingV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.ReserveBookingRequest type
    )
    {
        Result result = await sender.Send(new Command.ReserveBookingByIdCommand(id, type));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetBookingByIdV1(ISender sender, Guid bookingId)
    {
        Result<Response.GetBookingDetailResponse> result = await sender.Send(new Query.GetBookingByIdQuery(bookingId));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
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
