﻿using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Identity;
using BadmintonSystem.Domain.Enumerations;
using BadmintonSystem.Presentation.Abstractions;
using BadmintonSystem.Presentation.Extensions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Request = BadmintonSystem.Contract.Services.V1.Identity.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class AuthApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/auths";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("auths")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost("admin/register", RegisterV1)
            .RequireJwtAuthorize(FunctionEnum.ADMINISTRATOR.ToString(), (int)ActionEnum.CREATE);

        group1.MapPost("admin/reset-password", ResetPasswordV1)
            .RequireJwtAuthorize(FunctionEnum.ADMINISTRATOR.ToString(), (int)ActionEnum.CREATE);

        group1.MapPut("admin/reset-role-for-user", ResetRoleForUserV1)
            .RequireJwtAuthorize(FunctionEnum.ADMINISTRATOR.ToString(), (int)ActionEnum.UPDATE);

        group1.MapPut("admin/role-multiple-for-user", UpdateRoleMultipleForUserV1)
            .RequireJwtAuthorize(FunctionEnum.ADMINISTRATOR.ToString(), (int)ActionEnum.UPDATE);

        group1.MapPut("admin/role-claim", UpdateRoleClaimV1)
            .RequireJwtAuthorize(FunctionEnum.ADMINISTRATOR.ToString(), (int)ActionEnum.UPDATE);

        group1.MapPut("admin/user-claim", UpdateUserClaimV1)
            .RequireJwtAuthorize(FunctionEnum.ADMINISTRATOR.ToString(), (int)ActionEnum.UPDATE);

        group1.MapPost("admin/register-by-role", AdminRegisterV1)
            .RequireJwtAuthorize(FunctionEnum.ADMINISTRATOR.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("admin/{email}/get-authorization", GetUserAuthorizationByEmailV1)
            .RequireJwtAuthorize(FunctionEnum.ADMINISTRATOR.ToString(), (int)ActionEnum.READ);
    }

    private static async Task<IResult> RegisterV1(ISender sender, [FromBody] Request.RegisterRequest request)
    {
        Result result = await sender.Send(new Query.RegisterQuery(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> ResetPasswordV1
    (
        ISender sender,
        [FromBody] Request.ResetPasswordById request)
    {
        Result result = await sender.Send(new Command.ResetPasswordByIdCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetUserAuthorizationByEmailV1
    (
        ISender sender,
        string email)
    {
        Result result = await sender.Send(new Query.GetUserAuthorizationByEmailQuery(email));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> ResetRoleForUserV1
    (
        ISender sender,
        [FromBody] Request.ResetUserToDefaultRole request)
    {
        Result result = await sender.Send(new Command.ResetUserToDefaultRoleCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> AdminRegisterV1(ISender sender, [FromBody] Request.CreateAppUserRequest request)
    {
        Result result = await sender.Send(new Command.CreateAppUserCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateRoleMultipleForUserV1
    (
        ISender sender,
        [FromBody] Request.UpdateRoleMultipleForUserRequest request)
    {
        Result result = await sender.Send(new Command.UpdateRoleMultipleForUserCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateUserClaimV1
    (
        ISender sender,
        [FromBody] Request.UpdateAppUserClaimRequest request)
    {
        Result result = await sender.Send(new Command.UpdateAppUserClaimCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateRoleClaimV1
    (
        ISender sender,
        [FromBody] Request.UpdateAppRoleClaimRequest request)
    {
        Result result = await sender.Send(new Command.UpdateAppRoleClaimCommand(request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }
}
