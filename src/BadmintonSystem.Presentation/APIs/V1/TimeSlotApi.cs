using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.TimeSlot;
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
using Request = BadmintonSystem.Contract.Services.V1.TimeSlot.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class TimeSlotApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/time-slots";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("time slots")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateTimeSlotV1)
            .RequireJwtAuthorize(FunctionEnum.TIMESLOT.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("filter-and-sort", GetTimeSlotsFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.TIMESLOT.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{timeSlotId}", GetTimeSlotByIdV1)
            .RequireJwtAuthorize(FunctionEnum.TIMESLOT.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{timeSlotId}", UpdateTimeSlotV1)
            .RequireJwtAuthorize(FunctionEnum.TIMESLOT.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteTimeSlotsV1)
            .RequireJwtAuthorize(FunctionEnum.TIMESLOT.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateTimeSlotV1
    (
        ISender sender,
        [FromBody] Request.CreateTimeSlotRequest createTimeSlot,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.TimeSlotResponse> result =
            await sender.Send(new Command.CreateTimeSlotCommand(userId ?? Guid.Empty, createTimeSlot));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteTimeSlotsV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteTimeSlotsCommand(ids));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateTimeSlotV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateTimeSlotRequest updateTimeSlot
    )
    {
        updateTimeSlot.Id = id;
        Result<Response.TimeSlotResponse> result = await sender.Send(new Command.UpdateTimeSlotCommand(updateTimeSlot));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetTimeSlotByIdV1(ISender sender, Guid timeSlotId)
    {
        Result<Response.TimeSlotResponse> result = await sender.Send(new Query.GetTimeSlotByIdQuery(timeSlotId));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetTimeSlotsFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.TimeSlotDetailResponse>> result =
            await sender.Send(new Query.GetTimeSlotsWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
