using BadmintonSystem.Contract.Abstractions.Shared;
using BadmintonSystem.Contract.Services.V1.Review;
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
using Request = BadmintonSystem.Contract.Services.V1.Review.Request;

namespace BadmintonSystem.Presentation.APIs.V1;

public class ReviewApi : ApiEndpoint, ICarterModule
{
    private const string BaseUrl = "/api/v{version:apiVersion}/reviews";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group1 = app.NewVersionedApi("reviews")
            .MapGroup(BaseUrl)
            .HasApiVersion(1)
            .RequireAuthorization();

        group1.MapPost(string.Empty, CreateReviewV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet(string.Empty, GetReviewsV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.READ);
        group1.MapGet("filter-and-sort-value", GetReviewsFilterAndSortValueV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.READ);

        group1.MapGet("{reviewId}", GetReviewByIdV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.READ);

        group1.MapPut("{reviewId}", UpdateReviewV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete(string.Empty, DeleteReviewsV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateReviewV1
    (
        ISender sender,
        [FromBody] Request.CreateReviewRequest createReview,
        IHttpContextAccessor httpContextAccessor)
    {
        Guid? userId = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result<Response.ReviewResponse> result =
            await sender.Send(new Command.CreateReviewCommand(userId ?? Guid.Empty, createReview));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteReviewsV1(ISender sender, [FromBody] List<string> ids)
    {
        Result result = await sender.Send(new Command.DeleteReviewsCommand(ids));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateReviewV1
    (
        ISender sender,
        Guid id,
        [FromBody] Request.UpdateReviewRequest updateReview
    )
    {
        updateReview.Id = id;
        Result<Response.ReviewResponse> result = await sender.Send(new Command.UpdateReviewCommand(updateReview));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetReviewsV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedRequest request)
    {
        var pagedQueryRequest = new Contract.Abstractions.Shared.Request.PagedQueryRequest(request);
        Result<PagedResult<Response.ReviewResponse>> result =
            await sender.Send(new Query.GetReviewsQuery(pagedQueryRequest));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetReviewByIdV1(ISender sender, Guid reviewId)
    {
        Result<Response.ReviewResponse> result = await sender.Send(new Query.GetReviewByIdQuery(reviewId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetReviewsFilterAndSortValueV1
    (
        ISender sender,
        [AsParameters] Contract.Abstractions.Shared.Request.PagedFilterAndSortRequest request)
    {
        var pagedQueryRequest =
            new Contract.Abstractions.Shared.Request.PagedFilterAndSortQueryRequest(request);
        Result<PagedResult<Response.ReviewDetailResponse>> result =
            await sender.Send(new Query.GetReviewsWithFilterAndSortValueQuery(pagedQueryRequest));

        return Results.Ok(result);
    }
}
