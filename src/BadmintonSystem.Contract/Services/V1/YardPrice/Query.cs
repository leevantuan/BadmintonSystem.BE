using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;
using static BadmintonSystem.Contract.Services.V1.YardPrice.Request;

namespace BadmintonSystem.Contract.Services.V1.YardPrice;

public static class Query
{
    public record GetYardPriceByIdQuery(Guid Id)
        : IQuery<Response.YardPriceDetailResponse>;

    public record GetYardPricesWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.YardPriceDetailResponse>>;

    public record GetYardPricesByDateQuery(
        Guid UserId,
        DateTime Date,
        string Tenant)
        : IQuery<List<Response.YardPricesByDateDetailResponse>>;

    public record GetYardPricesByYardIdInTodayQuery(
        Guid UserId,
        Guid YardId)
        : IQuery<Response.YardPricesByDateDetailResponse>;

    // API chatbot
    public record GetYardPricesFreeByDateQuery(
        GetYardPricesFreeByDateRequest Data)
        : IQuery<List<Response.YardPricesFreeByDateDetailResponse>>;
}
