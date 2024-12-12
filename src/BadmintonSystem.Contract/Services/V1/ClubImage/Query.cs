using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.ClubImage;

public static class Query
{
    public record GetClubImagesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.ClubImageResponse>>;

    public record GetClubImageByIdQuery(Guid Id)
        : IQuery<Response.ClubImageResponse>;
}
