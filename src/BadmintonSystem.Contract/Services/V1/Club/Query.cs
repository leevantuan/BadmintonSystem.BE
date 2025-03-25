using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Club;

public static class Query
{
    public record GetClubsQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.ClubResponse>>;

    public record GetClubByIdQuery(Guid Id)
        : IQuery<Response.ClubDetailByIdResponse>;

    public record GetClubsWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.ClubDetailResponse>>;

    // API chat bot
    public record GetTopClubsQuery(int quantity)
    : IQuery<List<Response.ClubDetailResponseChatBot>>;
}
