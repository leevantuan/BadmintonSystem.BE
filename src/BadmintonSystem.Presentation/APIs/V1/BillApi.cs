using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Bill;
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
using Request = BadmintonSystem.Contract.Services.V1.Bill.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class BillApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/bills";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("bills")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        // QUERY
        group1.MapPost("filter-and-sort-value", GetBillsFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{billId}", GetBillByIdV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.READ);

        // Command
        group1.MapPost(string.Empty, CreateBillV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.UPDATE);

        group1.MapPut("update/{billId}", UpdateBillV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.DELETE);

        group1.MapPut("close-bill/{billId}", CloseBillV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.DELETE);

        group1.MapPut("open-yard", OpenYardByBillV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.DELETE);

        group1.MapPut("close-yard", CloseYardByBillV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.DELETE);

        group1.MapPut("update-quantity-service", UpdateQuantityServiceV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.DELETE);

        group1.MapPost("create-service/{billId}", CreateServiceByBillV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.DELETE);

        group1.MapDelete("{serviceLineId}", DeleteServiceBillV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.DELETE);

        group1.MapPut("open-booking/{billId}", OpenYardByBillInBookingV1)
            .RequireJwtAuthorize(FunctionEnum.BILL.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateBillV1
    (
        ISender sender,
        [FromBody] Request.CreateBillRequest createBill,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreateBillCommand(userId ?? Guid.Empty, createBill));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateBillV1
    (
        ISender sender,
        Guid billId,
        [FromBody] Request.UpdateBillRequest updateBill
    )
    {
        updateBill.Id = billId;
        Result result = await sender.Send(new Command.UpdateBillCommand(updateBill));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> CloseBillV1
    (
        ISender sender,
        Guid billId
    )
    {
        Result result = await sender.Send(new Command.CloseBillCommand(billId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> OpenYardByBillV1
    (
        ISender sender,
        [FromBody] Contract.Services.V1.BillLine.Request.CreateBillLineRequest data
    )
    {
        Result result = await sender.Send(new Command.OpenYardByBillCommand(data));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> CloseYardByBillV1
    (
        ISender sender,
        [FromBody] Contract.Services.V1.BillLine.Request.UpdateBillLineRequest data
    )
    {
        Result result = await sender.Send(new Command.CloseYardByBillCommand(data));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateQuantityServiceV1
    (
        ISender sender,
        [FromBody] Contract.Services.V1.BillLine.Request.UpdateQuantityServiceRequest data
    )
    {
        Result result = await sender.Send(new Command.UpdateQuantityServiceByBillCommand(data));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> CreateServiceByBillV1
    (
        ISender sender,
        Guid billId,
        [FromBody] List<Contract.Services.V1.ServiceLine.Request.CreateServiceLineRequest> data
    )
    {
        Result result = await sender.Send(new Command.CreateServicesByBillCommand(billId, data));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteServiceBillV1
    (
        ISender sender,
        Guid serviceLineId
    )
    {
        Result result = await sender.Send(new Command.DeleteServiceLineByBillCommand(serviceLineId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }


    private static async Task<IResult> OpenYardByBillInBookingV1
    (
        ISender sender,
        Guid billId
    )
    {
        Result result = await sender.Send(new Command.OpenYardByBillInBookingCommand(billId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetBillByIdV1(ISender sender, Guid billId)
    {
        Result<Response.BillDetailResponse> result = await sender.Send(new Query.GetBillByIdQuery(billId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetBillsFilterAndSortValueV1
    (
        ISender sender,
        [FromBody] Request.FilterBillRequest filter,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.BillDetailResponse>> result =
            await sender.Send(new Query.GetBillsWithFilterAndSortValueQuery(filter, pagedQueryRequest));

        return Results.Ok(result);
    }
}
