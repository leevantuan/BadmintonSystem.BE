using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.FixedSchedule;
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
using Request = BadmintonSystem.Contract.Services.V1.FixedSchedule.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class FixedScheduleApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/fixed-schedules";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("fixed schedules")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateFixedScheduleV1)
            .RequireJwtAuthorize(FunctionEnum.FIXEDSCHEDULE.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("filter-and-sort", GetFixedSchedulesFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.FIXEDSCHEDULE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{fixedScheduleId}", UpdateFixedScheduleV1)
            .RequireJwtAuthorize(FunctionEnum.FIXEDSCHEDULE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteFixedSchedulesV1)
            .RequireJwtAuthorize(FunctionEnum.FIXEDSCHEDULE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateFixedScheduleV1
    (
        ISender sender,
        [FromBody] Request.CreateFixedScheduleDetailRequest createFixedSchedule,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.FixedScheduleResponse> result =
            await sender.Send(new Command.CreateFixedScheduleCommand(userId ?? Guid.Empty, createFixedSchedule));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteFixedSchedulesV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteFixedSchedulesCommand(ids));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateFixedScheduleV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateFixedScheduleRequest updateFixedSchedule
    )
    {
        updateFixedSchedule.Id = id;
        Result<Response.FixedScheduleResponse> result =
            await sender.Send(new Command.UpdateFixedScheduleCommand(updateFixedSchedule));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetFixedSchedulesFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.GetFixedScheduleDetailResponse>> result =
            await sender.Send(new Query.GetFixedSchedulesWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
