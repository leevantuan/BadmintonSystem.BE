using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.ReviewImage;

public static class Query
{
    public record GetReviewImagesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.ReviewImageResponse>>;

    public record GetReviewImageByIdQuery(Guid Id)
        : IQuery<Response.ReviewImageResponse>;
}
