﻿using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Momo;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Request = BadmintonSystem.Contract.Services.V1.Momo.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class PaymentQRCodeApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/payment";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("payment")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateNotificationV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.NOTIFICATION.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("return", VerifyPaymentMomoV1)
            .AllowAnonymous();
    }

    private static async Task<IResult> VerifyPaymentMomoV1
    (
        ISender sender,
        HttpRequest request)
    {
        var query = request.Query;

        string amount = query["amount"];
        string orderId = query["orderId"];
        string orderInfo = query["orderInfo"];
        string resultCode = query["resultCode"];
        string message = query["message"];
        string signature = query["signature"];

        var momoPaymentResponse = new Contract.Services.V1.Payment.Request.PaymentRequest
        {
            OrderId = orderId,
            Type = !(resultCode == "0")
        };

        var result = await sender.Send(new Contract.Services.V1.Payment.Command.ReturnPaymentCommand(momoPaymentResponse));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
        //if (resultCode == "0")
        //{
        //    return Task.FromResult(Results.Redirect("https://bookingweb.shop"));
        //}

        //return Task.FromResult(Results.Ok("OK"));
    }

    private static async Task<IResult> CreateNotificationV1
    (
        ISender sender,
        [FromBody] Request.MomoPaymentRequest momoPaymentRequest)
    {
        Result<Response.MomoPaymentResponse> result =
            await sender.Send(new Command.CreateQRCodeCommand(momoPaymentRequest));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }
}
