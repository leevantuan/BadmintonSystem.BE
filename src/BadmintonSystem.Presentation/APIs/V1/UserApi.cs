using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
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
using Command = BadmintonSystem.Contract.Services.V1.User.Command;
using Request = BadmintonSystem.Contract.Services.V1.User.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class UserApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/users";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("users")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost("login", LoginV1).AllowAnonymous();
        group1.MapPost("register", RegisterV1).AllowAnonymous();
        group1.MapPost("forget-password", ForgetPasswordV1).AllowAnonymous();

        group1.MapPut("change-password", ChangePasswordV1)
            .RequireJwtAuthorize(FunctionEnum.USER.ToString(), (int)ActionEnum.UPDATE);

        // ADDRESS
        group1.MapGet("addresses", GetAddressByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.ADDRESS.ToString(), (int)ActionEnum.READ);

        // PAYMENT METHOD
        group1.MapGet("payment-methods", GetPaymentMethodByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.USER.ToString(), (int)ActionEnum.READ);

        // NOTIFICATION
        group1.MapGet("notifications", GetNotificationsByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.NOTIFICATION.ToString(), (int)ActionEnum.READ);

        // REVIEW
        group1.MapGet("reviews", GetReviewByUserIdWithFilterAndSortV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.READ);

        // BOOKING
        group1.MapGet("bookings", GetBookingByUserIdWithFilterAndSortV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.READ);

        // CHAT MESSAGE
        group1.MapPost("chat-message", GetChatMessageByUserIdWithFilterAndSortV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.READ);

        // VERIFICATION EMAIL
        group1.MapGet("verify-email", VerificationEmailV1)
            .AllowAnonymous();
    }

    private static async Task<IResult> VerificationEmailV1(ISender sender, [FromQuery] Guid userId)
    {
        Result result =
            await sender.Send(new Contract.Services.V1.User.Query.VerificationEmailWhenRegisterQuery(userId));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        string redirectUrl = "https://bookingweb.shop/verify-email";

        return Results.Redirect(redirectUrl);
    }

    private static async Task<IResult> LoginV1(ISender sender, [FromBody] Query.LoginQuery login)
    {
        Result<Response.LoginResponse> result = await sender.Send(login);

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> RegisterV1(ISender sender, [FromBody] Request.CreateUserAndAddress request)
    {
        Result result = await sender.Send(new Contract.Services.V1.User.Query.RegisterByCustomerQuery(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> ForgetPasswordV1
    (
        ISender sender,
        [FromBody] Request.ForgetPasswordRequest request)
    {
        Result result = await sender.Send(new Command.ForgetPasswordCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> ChangePasswordV1
    (
        ISender sender,
        [FromBody] Request.ChangePasswordRequest request)
    {
        Result result = await sender.Send(new Command.ChangePasswordCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetAddressByUserIdV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();

        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Contract.Services.V1.User.Response.AddressByUserDetailResponse>> result =
            await sender.Send(
                new Contract.Services.V1.User.Query.GetAddressesByEmailWithFilterAndSortQuery(
                    userIdCurrent ?? Guid.Empty, pagedQueryRequest));

        return Results.Ok(result);
    }

    private static async Task<IResult> GetPaymentMethodByUserIdV1
    (
        ISender sender,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();

        Result<PagedResult<Contract.Services.V1.User.Response.PaymentMethodByUserResponse>> result =
            await sender.Send(
                new Contract.Services.V1.User.Query.GetPaymentMethodsByUserIdQuery(
                    userIdCurrent ?? Guid.Empty));

        return Results.Ok(result);
    }

    // NOTIFICATION ================================================
    private static async Task<IResult> GetNotificationsByUserIdV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();

        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);

        Result<PagedResult<Contract.Services.V1.User.Response.NotificationByUserResponse>> result =
            await sender.Send(
                new Contract.Services.V1.User.Query.GetNotificationsByUserIdWithFilterAndSortQuery(
                    userIdCurrent ?? Guid.Empty, pagedQueryRequest));

        return Results.Ok(result);
    }

    private static async Task<IResult> GetReviewByUserIdWithFilterAndSortV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();

        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);

        Result<PagedResult<Contract.Services.V1.User.Response.ReviewByUserResponse>> result =
            await sender.Send(
                new Contract.Services.V1.User.Query.GetReviewsByUserIdWithFilterAndSortQuery(
                    userIdCurrent ?? Guid.Empty, pagedQueryRequest));

        return Results.Ok(result);
    }

    private static async Task<IResult> GetBookingByUserIdWithFilterAndSortV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();

        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);

        Result<PagedResult<Contract.Services.V1.User.Response.GetBookingByUserIdResponse>> result =
            await sender.Send(
                new Contract.Services.V1.User.Query.GetBookingsByUserIdWithFilterAndSortQuery(
                    userIdCurrent ?? Guid.Empty, pagedQueryRequest));

        return Results.Ok(result);
    }

    private static async Task<IResult> GetChatMessageByUserIdWithFilterAndSortV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request,
        [FromBody] Request.GetChatMessageRequest data,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userIdCurrent = data.UserId ?? httpContextAccessor.HttpContext?.GetCurrentUserId();

        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);

        Result<PagedResult<Contract.Services.V1.User.Response.GetChatMessageByUserIdResponse>> result =
            await sender.Send(
                new Contract.Services.V1.User.Query.GetChatMessagesByUserIdWithFilterAndSortQuery(
                    userIdCurrent ?? Guid.Empty, pagedQueryRequest));

        return Results.Ok(result);
    }
}
