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
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.UPDATE);

        // ADDRESS
        group1.MapPost("address", CreateAddressByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.CREATE);

        group1.MapPut("address", UpdateAddressByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.UPDATE);

        group1.MapGet("addresses", GetAddressByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.READ);

        group1.MapDelete("address/{addressId}", DeleteAddressByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.DELETE);

        // PAYMENT METHOD
        group1.MapPost("payment-method", CreatePaymentMethodByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("payment-methods", GetPaymentMethodByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.READ);
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

    private static async Task<IResult> CreateAddressByUserIdV1
    (
        ISender sender,
        [FromBody] Contract.Services.V1.Address.Request.CreateAddressRequest request,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreateAddressByUserIdCommand(userIdCurrent ?? Guid.Empty, request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateAddressByUserIdV1
    (
        ISender sender,
        [FromBody] Request.UpdateAddressByUserIdRequest request,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.UpdateAddressByUserIdCommand(userIdCurrent ?? Guid.Empty, request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteAddressByUserIdV1
    (
        ISender sender,
        Guid addressId,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.DeleteAddressByUserIdCommand(userIdCurrent ?? Guid.Empty, addressId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> CreatePaymentMethodByUserIdV1
    (
        ISender sender,
        [FromBody] Contract.Services.V1.PaymentMethod.Request.CreatePaymentMethodRequest request,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreatePaymentMethodByUserIdCommand(userIdCurrent ?? Guid.Empty, request));

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
}
