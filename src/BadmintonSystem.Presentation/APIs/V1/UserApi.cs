using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.User;
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

        group1.MapPost("register", RegisterV1).AllowAnonymous();

        group1.MapGet("addresses", GetAddressByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.READ);

        group1.MapPost("address", CreateAddressByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.CREATE);
    }

    private static async Task<IResult> RegisterV1(ISender sender, [FromBody] Request.CreateUserAndAddress request)
    {
        Result result = await sender.Send(new Query.RegisterByCustomerQuery(request));

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

    private static async Task<IResult> GetAddressByUserIdV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();

        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.AddressByUserDetailResponse>> result =
            await sender.Send(
                new Query.GetAddressesByEmailWithFilterAndSortQuery(userIdCurrent ?? Guid.Empty, pagedQueryRequest));

        return Results.Ok(result);
    }
}
