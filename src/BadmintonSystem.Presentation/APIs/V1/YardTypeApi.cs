using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardType;
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
using Request = BadmintonSystem.Contract.Services.V1.YardType.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class YardTypeApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/yard-types";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("yard types")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateYardTypeV1)
            .RequireJwtAuthorize(FunctionEnum.YARDTYPE.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("filter-and-sort-value", GetYardTypesFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.YARDTYPE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{yardTypeId}", GetYardTypeByIdV1)
            .RequireJwtAuthorize(FunctionEnum.YARDTYPE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{yardTypeId}", UpdateYardTypeV1)
            .RequireJwtAuthorize(FunctionEnum.YARDTYPE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteYardTypesV1)
            .RequireJwtAuthorize(FunctionEnum.YARDTYPE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateYardTypeV1
    (
        ISender sender,
        [FromBody] Request.CreateYardTypeRequest createYardType,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.YardTypeResponse> result =
            await sender.Send(new Command.CreateYardTypeCommand(userId ?? Guid.Empty, createYardType));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteYardTypesV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteYardTypesCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateYardTypeV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateYardTypeRequest updateYardType
    )
    {
        updateYardType.Id = id;
        Result<Response.YardTypeResponse> result = await sender.Send(new Command.UpdateYardTypeCommand(updateYardType));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetYardTypeByIdV1(ISender sender, Guid yardTypeId)
    {
        Result<Response.YardTypeResponse> result = await sender.Send(new Query.GetYardTypeByIdQuery(yardTypeId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetYardTypesFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.YardTypeDetailResponse>> result =
            await sender.Send(new Query.GetYardTypesWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
