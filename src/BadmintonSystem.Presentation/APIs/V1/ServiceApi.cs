using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Service;
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
using Request = BadmintonSystem.Contract.Services.V1.Service.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class ServiceApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/services";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("services")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateServiceV1)
            .RequireJwtAuthorize(FunctionEnum.SERVICE.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet(string.Empty, GetServicesV1)
            .RequireJwtAuthorize(FunctionEnum.SERVICE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("filter-and-sort-value", GetServicesFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.SERVICE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("inventory-receipts/{serviceId}", GetInventoryReceiptByServiceIdFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.SERVICE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{serviceId}", GetServiceByIdV1)
            .RequireJwtAuthorize(FunctionEnum.SERVICE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{serviceId}", UpdateServiceV1)
            .RequireJwtAuthorize(FunctionEnum.SERVICE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapPut("update-quantity", UpdateServiceQuantityV1)
            .RequireJwtAuthorize(FunctionEnum.SERVICE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteServicesV1)
            .RequireJwtAuthorize(FunctionEnum.SERVICE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateServiceV1
    (
        ISender sender,
        [FromBody] Request.CreateServiceRequest createService, IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreateServicesCommand(userId ?? Guid.Empty, createService));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteServicesV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteServicesCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateServiceV1
    (
        ISender sender, Guid id,
        [FromBody] Request.UpdateServiceRequest updateService)
    {
        updateService.Id = id;
        Result result = await sender.Send(new Command.UpdateServiceCommand(updateService));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateServiceQuantityV1
    (
        ISender sender,
        [FromBody] Request.UpdateServiceQuantityRequest updateService)
    {
        Result result = await sender.Send(new Command.UpdateQuantityServiceCommand(updateService));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetServicesV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedRequest request)
    {
        var pagedQueryRequest = new Contract.Abstractions.Shared.Request.PagedQueryRequest(request);
        Result<PagedResult<Response.ServiceResponse>> result =
            await sender.Send(new Query.GetServicesQuery(pagedQueryRequest));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetServiceByIdV1(ISender sender, Guid serviceId)
    {
        Result<Response.ServiceResponse> result = await sender.Send(new Query.GetServiceByIdQuery(serviceId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetServicesFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.ServiceDetailResponse>> result =
            await sender.Send(new Query.GetServicesWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }

    private static async Task<IResult> GetInventoryReceiptByServiceIdFilterAndSortValueV1
    (
        ISender sender,
        Guid serviceId,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.GetInventoryReceiptByServiceIdResponse>> result =
            await sender.Send(
                new Query.GetInventoryReceiptsByServiceIdWithFilterAndSortValueQuery(serviceId, pagedQueryRequest));

        return Results.Ok(result);
    }
}
