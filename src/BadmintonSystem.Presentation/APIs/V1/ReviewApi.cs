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

        group1.MapPost(string.Empty, CreateReviewByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.CREATE);

        group1.MapGet("{reviewId}", GetReviewByIdV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.READ);

        group1.MapPut(string.Empty, UpdateReviewByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.UPDATE);

        group1.MapDelete("{reviewId}", DeleteReviewByUserIdV1)
            .RequireJwtAuthorize(FunctionEnum.REVIEW.ToString(), (int)ActionEnum.DELETE);
    }

    private static async Task<IResult> CreateReviewByUserIdV1
    (
        ISender sender,
        [FromBody] Request.CreateReviewByUserIdRequest request,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.CreateReviewByUserIdCommand(userIdCurrent ?? Guid.Empty, request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> UpdateReviewByUserIdV1
    (
        ISender sender,
        [FromBody] Request.UpdateReviewByUserIdRequest request,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(new Command.UpdateReviewByUserIdCommand(userIdCurrent ?? Guid.Empty, request));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> DeleteReviewByUserIdV1
    (
        ISender sender,
        Guid reviewId,
        IHttpContextAccessor httpContextAccessor
    )
    {
        Guid? userIdCurrent = httpContextAccessor.HttpContext?.GetCurrentUserId();
        Result result =
            await sender.Send(
                new Command.DeleteReviewByUserIdCommand(userIdCurrent ?? Guid.Empty, reviewId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }

    private static async Task<IResult> GetReviewByIdV1(ISender sender, Guid reviewId)
    {
        Result<Response.GetReviewDetailResponse> result = await sender.Send(new Query.GetReviewByIdQuery(reviewId));

        return result.IsFailure ? HandleFailure(result) : Results.Ok(result);
    }
}
