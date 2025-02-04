using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Club;
using BadmintonSystem.Persistence.Helpers;
using BadmintonSystem.Presentation.Abstractions;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Request = BadmintonSystem.Contract.Services.V1.Club.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class ClubApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/clubs";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("clubs")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateClubV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CLUB.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet(string.Empty, GetClubsV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CLUB.ToString(), (int)ActionEnum.READ);
        group1.MapGet("filter-and-sort", GetClubsFilterAndSortValueV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CLUB.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{clubId}", GetClubByIdV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CLUB.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{clubId}", UpdateClubV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CLUB.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteClubsV1)
            .AllowAnonymous();
        //.RequireJwtAuthorize(FunctionEnum.CLUB.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateClubV1
    (
        ISender sender,
        [FromBody] Request.CreateClubDetailsRequest createClub,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.ClubResponse> result =
            await sender.Send(new Command.CreateClubCommand(userId ?? Guid.Empty, createClub));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteClubsV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteClubsCommand(ids));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateClubV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateClubDetailsRequest updateClub
    )
    {
        updateClub.Id = id;
        Result<Response.ClubResponse> result = await sender.Send(new Command.UpdateClubCommand(updateClub));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetClubsV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedRequest request)
    {
        var pagedQueryRequest = new Contract.Abstractions.Shared.Request.PagedQueryRequest(request);
        Result<PagedResult<Response.ClubResponse>> result =
            await sender.Send(new Query.GetClubsQuery(pagedQueryRequest));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetClubByIdV1(ISender sender, Guid clubId)
    {
        Result<Response.ClubDetailResponse> result = await sender.Send(new Query.GetClubByIdQuery(clubId));

        return result.IsFailure ? HandleFailureConvertOk(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetClubsFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.ClubDetailResponse>> result =
            await sender.Send(new Query.GetClubsWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
