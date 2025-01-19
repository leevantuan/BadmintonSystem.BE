using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Yard;
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
using Request = BadmintonSystem.Contract.Services.V1.Yard.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class YardApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/yards";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("yards")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateYardV1)
            .RequireJwtAuthorize(FunctionEnum.YARD.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("filter-and-sort", GetYardsFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.YARD.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{yardId}", GetYardByIdV1)
            .RequireJwtAuthorize(FunctionEnum.YARD.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{yardId}", UpdateYardV1)
            .RequireJwtAuthorize(FunctionEnum.YARD.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteYardsV1)
            .RequireJwtAuthorize(FunctionEnum.YARD.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateYardV1
    (
        ISender sender,
        [FromBody] Request.CreateYardRequest createYard,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.YardResponse> result =
            await sender.Send(new Command.CreateYardCommand(userId ?? Guid.Empty, createYard));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteYardsV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteYardsCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateYardV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateYardRequest updateYard
    )
    {
        updateYard.Id = id;
        Result<Response.YardResponse> result = await sender.Send(new Command.UpdateYardCommand(updateYard));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetYardByIdV1(ISender sender, Guid yardId)
    {
        Result<Response.YardResponse> result = await sender.Send(new Query.GetYardByIdQuery(yardId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetYardsFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.YardDetailResponse>> result =
            await sender.Send(new Query.GetYardsWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
