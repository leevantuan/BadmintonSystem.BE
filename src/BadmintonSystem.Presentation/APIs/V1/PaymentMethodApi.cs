using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.PaymentMethod;
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
using Request = BadmintonSystem.Contract.Services.V1.PaymentMethod.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class PaymentMethodApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/payment-methods";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("payment methods")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreatePaymentMethodByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("{paymentMethodId}", GetPaymentMethodByIdV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.READ);

        group1.MapPut(string.Empty, UpdatePaymentMethodByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete("{paymentMethodId}", DeletePaymentMethodByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreatePaymentMethodByUserIdV1
    (
        ISender sender,
        [FromBody] Request.CreatePaymentMethodRequest request,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreatePaymentMethodByUserIdCommand(userIdCurrent ?? Guid.Empty, request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdatePaymentMethodByUserIdV1
    (
        ISender sender,
        [FromBody] Request.UpdatePaymentMethodRequest request,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.UpdatePaymentMethodByUserIdCommand(userIdCurrent ?? Guid.Empty, request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeletePaymentMethodByUserIdV1
    (
        ISender sender,
        Guid paymentMethodId,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(
                new Command.DeletePaymentMethodByUserIdCommand(userIdCurrent ?? Guid.Empty, paymentMethodId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetPaymentMethodByIdV1(ISender sender, Guid paymentMethodId)
    {
        Result<Response.PaymentMethodDetailResponse> result =
            await sender.Send(new Query.GetPaymentMethodByIdQuery(paymentMethodId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }
}
