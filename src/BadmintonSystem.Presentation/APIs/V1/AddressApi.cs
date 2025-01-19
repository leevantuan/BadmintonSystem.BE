using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Address;
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
using Request = BadmintonSystem.Contract.Services.V1.Address.Request;
using Response = BadmintonSystem.Contract.Services.V1.User.Response;

namespace BadmintonSystem.Presentation.APIs.V1;

public class AddressApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/addresss";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("addresss")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateAddressV1)
            .RequireJwtAuthorize(FunctionEnum.ADDRESS.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("{addressId}", GetAddressByIdV1)
            .RequireJwtAuthorize(FunctionEnum.ADDRESS.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{addressId}", UpdateAddressV1)
            .RequireJwtAuthorize(FunctionEnum.ADDRESS.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete("{addressId}", DeleteAddressByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.ADDRESS.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateAddressV1
    (
        ISender sender,
        [FromBody] Request.CreateAddressRequest createAddress,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreateAddressByUserIdCommand(userId ?? Guid.Empty, createAddress));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateAddressV1
    (
        ISender sender,
        [FromBody] Request.UpdateAddressByUserIdRequest updateAddress,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.UpdateAddressByUserIdCommand(userId ?? Guid.Empty, updateAddress));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetAddressByIdV1
        (ISender sender, Guid addressId, IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();

        Result<Response.AddressByUserDetailResponse> result =
            await sender.Send(new Query.GetAddressesByIdQuery(userId ?? Guid.Empty, addressId));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
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
            await sender.Send(
                new Command.DeleteAddressByUserIdCommand(userIdCurrent ?? Guid.Empty, addressId));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }
}
