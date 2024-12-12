using BadmintonSystem.Contract.Abstractions.Shared;
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

        group1.MapPost("login", AuthenticationV1).AllowAnonymous();
        group1.MapPost("register", RegisterV1).AllowAnonymous();

        group1.MapPut("admin/reset-role-for-user", ResetRoleForUserV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.UPDATE);
        group1.MapPut("admin/role-multiple-for-user", UpdateRoleMultipleForUserV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.UPDATE);
        group1.MapPut("admin/role-claim", UpdateRoleClaimV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.UPDATE);
        group1.MapPut("admin/user-claim", UpdateUserClaimV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.UPDATE);
        group1.MapPost("admin/register", AdminRegisterV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.CREATE);
        group1.MapGet("admin/get-user-authorization", GetUserAuthorizationByEmailV1)
            .RequireJwtAuthorize(FunctionEnum.APPUSER.ToString(), (int)ActionEnum.READ);

        group1.MapPost("role", CreateAppRoleV1).AllowAnonymous();
        group1.MapPost("role-claim", CreateAppRoleClaimV1).AllowAnonymous();
        group1.MapPost("function", CreateFunctionV1).AllowAnonymous();
        group1.MapPost("action", CreateActionV1).AllowAnonymous();
    }

    public static async Task<IResult> AuthenticationV1(ISender sender, [FromBody] Query.LoginQuery login)
    {
        Result<Response.Authenticated> result = await sender.Send(login);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }

    public static async Task<IResult> RegisterV1(ISender sender, [FromBody] Request.RegisterRequest request)
    {
        Result result = await sender.Send(new Query.RegisterQuery(request));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }

    public static async Task<IResult> GetUserAuthorizationByEmailV1(ISender sender,
        string Email)
    {
        Result result = await sender.Send(new Query.GetUserAuthorizationByEmailQuery(Email));

        return Results.Ok(result);
    }

    public static async Task<IResult> ResetRoleForUserV1(ISender sender,
        [FromBody] Request.ResetUserToDefaultRole request)
    {
        Result result = await sender.Send(new Command.ResetUserToDefaultRoleCommand(request));

        return Results.Ok(result);
    }

    public static async Task<IResult> AdminRegisterV1(ISender sender, [FromBody] Request.CreateAppUserRequest request)
    {
        Result result = await sender.Send(new Command.CreateAppUserCommand(request));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateRoleMultipleForUserV1(ISender sender,
        [FromBody] Request.UpdateRoleMultipleForUserRequest request)
    {
        Result result = await sender.Send(new Command.UpdateRoleMultipleForUserCommand(request));

        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateUserClaimV1(ISender sender,
        [FromBody] Request.UpdateAppUserClaimRequest request)
    {
        Result result = await sender.Send(new Command.UpdateAppUserClaimCommand(request));

        return Results.Ok(result);
    }

    public static async Task<IResult> UpdateRoleClaimV1(ISender sender,
        [FromBody] Request.UpdateAppRoleClaimRequest request)
    {
        Result result = await sender.Send(new Command.UpdateAppRoleClaimCommand(request));

        return Results.Ok(result);
    }

    public static async Task<IResult> CreateAppRoleV1(ISender sender, [FromBody] Request.CreateAppRoleRequest request)
    {
        Result result = await sender.Send(new Command.CreateAppRoleCommand(request));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }

    public static async Task<IResult> CreateAppRoleClaimV1(ISender sender,
        [FromBody] Request.CreateAppRoleClaimRequest request)
    {
        Result result = await sender.Send(new Command.CreateAppRoleClaimCommand(request));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }

    public static async Task<IResult> CreateFunctionV1(ISender sender, [FromBody] Request.CreateFunctionRequest request)
    {
        Result result = await sender.Send(new Command.CreateFunctionCommand(request));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }

    public static async Task<IResult> CreateActionV1(ISender sender, [FromBody] Request.CreateActionRequest request)
    {
        Result result = await sender.Send(new Command.CreateActionCommand(request));

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Results.Ok(result);
    }
}
