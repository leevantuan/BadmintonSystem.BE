﻿using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.DayOff;
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
using Request = BadmintonSystem.Contract.Services.V1.DayOff.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class DayOffApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/day-off";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("day off")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateDayOffV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.CREATE);

        // group1.MapGet("filter-and-sort-value", GetDayOffsFilterAndSortValueV1)
        //     .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.READ);

        // group1.MapGet("{dayOffId}", GetDayOffByIdV1)
        //     .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{dayOffId}", UpdateDayOffV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteDayOffsV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateDayOffV1
    (
        ISender sender,
        [FromBody] Request.CreateDayOffRequest createDayOff,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.DayOffResponse> result =
            await sender.Send(new Command.CreateDayOffCommand(userId ?? Guid.Empty, createDayOff));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteDayOffsV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteDayOffsCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateDayOffV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateDayOffRequest updateDayOff
    )
    {
        updateDayOff.Id = id;
        Result<Response.DayOffResponse> result = await sender.Send(new Command.UpdateDayOffCommand(updateDayOff));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetDayOffByIdV1(ISender sender, Guid dayOffId)
    {
        Result<Response.DayOffDetailResponse> result = await sender.Send(new Query.GetDayOffByIdQuery(dayOffId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetDayOffsFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.DayOffDetailResponse>> result =
            await sender.Send(new Query.GetDayOffsWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}