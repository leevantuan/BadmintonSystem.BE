using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Price;
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
using Request = BadmintonSystem.Contract.Services.V1.Price.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class PriceApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/prices";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("prices")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreatePriceV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("filter-and-sort-value", GetPricesFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{priceId}", GetPriceByIdV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{priceId}", UpdatePriceV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeletePricesV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreatePriceV1
    (
        ISender sender,
        [FromBody] Request.CreatePriceRequest createPrice,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.PriceResponse> result =
            await sender.Send(new Command.CreatePriceCommand(userId ?? Guid.Empty, createPrice));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeletePricesV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeletePricesCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdatePriceV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdatePriceRequest updatePrice
    )
    {
        updatePrice.Id = id;
        Result<Response.PriceResponse> result = await sender.Send(new Command.UpdatePriceCommand(updatePrice));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetPriceByIdV1(ISender sender, Guid priceId)
    {
        Result<Response.PriceDetailResponse> result = await sender.Send(new Query.GetPriceByIdQuery(priceId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetPricesFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.PriceDetailResponse>> result =
            await sender.Send(new Query.GetPricesWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
