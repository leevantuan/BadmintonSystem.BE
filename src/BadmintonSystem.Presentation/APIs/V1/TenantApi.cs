using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace BadmintonSystem.Presentation.APIs.V1;

public class TenantApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/tenants";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("tenants")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateTenantV1)
            .AllowAnonymous();

        group1.MapPut(string.Empty, UpdateTenantV1)
            .AllowAnonymous();

        group1.MapGet(string.Empty, GetAllTenantV1)
            .AllowAnonymous();

        group1.MapGet("{tenantId}", GetByIdTenantV1)
            .AllowAnonymous();

        group1.MapGet("get-by-tenant-name/{tenantName}", GetContainTenantNameV1)
            .AllowAnonymous();
    }

    private static async Task<IResult> GetContainTenantNameV1
    (
        ISender sender,
        string tenantName)
    {
        Result result = await sender.Send(new Contract.Services.V1.Tenant.Query.GetTenantContainTenantNameQuery(tenantName));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> CreateTenantV1
    (
        ISender sender,
        [FromBody] Contract.Services.V1.Tenant.Request.CreateTenantRequest request)
    {
        Result result = await sender.Send(new Contract.Services.V1.Tenant.Command.CreateTenantCommand(request));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateTenantV1
    (
        ISender sender,
        [FromBody] Contract.Services.V1.Tenant.Request.UpdateTenantRequest request)
    {
        Result result = await sender.Send(new Contract.Services.V1.Tenant.Command.UpdaterTenantCommand(request));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetAllTenantV1
    (
        ISender sender)
    {
        Result result = await sender.Send(new Contract.Services.V1.Tenant.Query.GetTenantsQuery());

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetByIdTenantV1
    (
        ISender sender,
        Guid tenantId)
    {
        Result result = await sender.Send(new Contract.Services.V1.Tenant.Query.GetTenantByIdQuery(tenantId));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }
}
