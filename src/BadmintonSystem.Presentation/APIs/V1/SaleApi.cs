using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Sale;
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
using Request = BadmintonSystem.Contract.Services.V1.Sale.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class SaleApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/sales";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("sales")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateSaleV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet(string.Empty, GetSalesV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.READ);
        group1.MapGet("filter-and-sort-value", GetSalesFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{saleId}", GetSaleByIdV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{saleId}", UpdateSaleV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteSalesV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateSaleV1
    (
        ISender sender,
        [FromBody] Request.CreateSaleRequest createSale,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.SaleResponse> result =
            await sender.Send(new Command.CreateSaleCommand(userId ?? Guid.Empty, createSale));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteSalesV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteSalesCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateSaleV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateSaleRequest updateSale
    )
    {
        updateSale.Id = id;
        Result<Response.SaleResponse> result = await sender.Send(new Command.UpdateSaleCommand(updateSale));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetSalesV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedRequest request)
    {
        var pagedQueryRequest = new Contract.Abstractions.Shared.Request.PagedQueryRequest(request);
        Result<PagedResult<Response.SaleResponse>> result =
            await sender.Send(new Query.GetSalesQuery(pagedQueryRequest));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetSaleByIdV1(ISender sender, Guid saleId)
    {
        Result<Response.SaleResponse> result = await sender.Send(new Query.GetSaleByIdQuery(saleId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetSalesFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.SaleDetailResponse>> result =
            await sender.Send(new Query.GetSalesWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
