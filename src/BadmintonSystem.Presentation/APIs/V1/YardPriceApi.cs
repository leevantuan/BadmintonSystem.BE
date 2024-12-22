using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.YardPrice;
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
using Request = BadmintonSystem.Contract.Services.V1.YardPrice.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class YardPriceApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/yard-prices";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("yard prices")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateYardPriceV1)
            .RequireJwtAuthorize(FunctionEnum.YARDPRICE.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("filter-and-sort-value", GetYardPricesFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.YARDPRICE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{yardPriceId}", GetYardPriceByIdV1)
            .RequireJwtAuthorize(FunctionEnum.YARDPRICE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{yardPriceId}", UpdateYardPriceV1)
            .RequireJwtAuthorize(FunctionEnum.YARDPRICE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteYardPricesV1)
            .RequireJwtAuthorize(FunctionEnum.YARDPRICE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateYardPriceV1
    (
        ISender sender,
        [FromBody] Request.CreateYardPriceRequest createYardPrice,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.YardPriceResponse> result =
            await sender.Send(new Command.CreateYardPriceCommand(userId ?? Guid.Empty, createYardPrice));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteYardPricesV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteYardPricesCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateYardPriceV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateYardPriceRequest updateYardPrice
    )
    {
        updateYardPrice.Id = id;
        Result<Response.YardPriceResponse> result = await sender.Send(new Command.UpdateYardPriceCommand(updateYardPrice));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetYardPriceByIdV1(ISender sender, Guid yardPriceId)
    {
        Result<Response.YardPriceResponse> result = await sender.Send(new Query.GetYardPriceByIdQuery(yardPriceId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetYardPricesFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.YardPriceDetailResponse>> result =
            await sender.Send(new Query.GetYardPricesWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
