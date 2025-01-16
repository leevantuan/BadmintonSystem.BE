using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.InventoryReceipt;
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
using Request = BadmintonSystem.Contract.Services.V1.InventoryReceipt.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class InventoryReceiptApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/inventory-receipts";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("inventory receipts")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateInventoryReceiptV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.CREATE);

        group1.MapPost("filter-and-sort-value", GetInventoryReceiptsFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{inventoryReceiptId}", GetInventoryReceiptByIdV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.READ);

        group1.MapDelete("{inventoryReceiptId}", DeleteInventoryReceiptsV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateInventoryReceiptV1
    (
        ISender sender,
        [FromBody] Request.CreateInventoryReceiptRequest createInventoryReceipt,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreateInventoryReceiptCommand(userId ?? Guid.Empty, createInventoryReceipt));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteInventoryReceiptsV1(ISender sender, Guid inventoryReceiptId)
    {
        Result result = await sender.Send(new Command.DeleteInventoryReceiptsCommand(inventoryReceiptId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetInventoryReceiptByIdV1(ISender sender, Guid inventoryReceiptId)
    {
        Result<Response.InventoryReceiptDetailResponse> result =
            await sender.Send(new Query.GetInventoryReceiptByIdQuery(inventoryReceiptId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetInventoryReceiptsFilterAndSortValueV1
    (
        ISender sender,
        [FromBody] Request.FilterInventoryReceiptRequest filter,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.InventoryReceiptDetailResponse>> result =
            await sender.Send(new Query.GetInventoryReceiptsWithFilterAndSortValueQuery(filter, pagedQueryRequest));

        return Results.Ok(result);
    }
}
