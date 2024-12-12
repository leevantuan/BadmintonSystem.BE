using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Notification;
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
using Request = BadmintonSystem.Contract.Services.V1.Notification.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class NotificationApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/notifications";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("notifications")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateNotificationV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet(string.Empty, GetNotificationsV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.READ);
        group1.MapGet("filter-and-sort-value", GetNotificationsFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{notificationId}", GetNotificationByIdV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{notificationId}", UpdateNotificationV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteNotificationsV1)
            .RequireJwtAuthorize(FunctionEnum.SALE.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateNotificationV1
    (
        ISender sender,
        [FromBody] Request.CreateNotificationRequest createNotification,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.NotificationResponse> result =
            await sender.Send(new Command.CreateNotificationCommand(userId ?? Guid.Empty, createNotification));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteNotificationsV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteNotificationsCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateNotificationV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateNotificationRequest updateNotification
    )
    {
        updateNotification.Id = id;
        Result<Response.NotificationResponse> result =
            await sender.Send(new Command.UpdateNotificationCommand(updateNotification));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetNotificationsV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedRequest request)
    {
        var pagedQueryRequest = new Contract.Abstractions.Shared.Request.PagedQueryRequest(request);
        Result<PagedResult<Response.NotificationResponse>> result =
            await sender.Send(new Query.GetNotificationsQuery(pagedQueryRequest));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetNotificationByIdV1(ISender sender, Guid notificationId)
    {
        Result<Response.NotificationResponse> result =
            await sender.Send(new Query.GetNotificationByIdQuery(notificationId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetNotificationsFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.NotificationDetailResponse>> result =
            await sender.Send(new Query.GetNotificationsWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
