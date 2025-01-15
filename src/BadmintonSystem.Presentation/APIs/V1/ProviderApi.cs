using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Provider;
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
using Request = BadmintonSystem.Contract.Services.V1.Provider.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class ProviderApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/providers";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("providers")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateProviderV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("inventory-receipts/{providerId}",
                GetInventoryReceiptsByProviderIdWithFilterAndSort)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("filter-and-sort-value", GetProvidersFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{providerId}", GetProviderByIdV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{providerId}", UpdateProviderV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteProvidersV1)
            .RequireJwtAuthorize(FunctionEnum.PRICE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateProviderV1
    (
        ISender sender,
        [FromBody] Request.CreateProviderRequest createProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreateProviderCommand(userId ?? Guid.Empty, createProvider));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteProvidersV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteProvidersCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateProviderV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateProviderRequest updateProvider
    )
    {
        updateProvider.Id = id;
        Result result = await sender.Send(new Command.UpdateProviderCommand(updateProvider));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetProviderByIdV1(ISender sender, Guid providerId)
    {
        Result<Response.ProviderDetailResponse> result = await sender.Send(new Query.GetProviderByIdQuery(providerId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetProvidersFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.ProviderDetailResponse>> result =
            await sender.Send(new Query.GetProvidersWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }

    private static async Task<IResult> GetInventoryReceiptsByProviderIdWithFilterAndSort
    (
        ISender sender,
        Guid providerId,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.GetInventoryReceiptByProviderIdResponse>> result =
            await sender.Send(
                new Query.GetInventoryReceiptsByProviderIdWithFilterAndSortValueQuery(providerId, pagedQueryRequest));

        return Results.Ok(result);
    }
}
