﻿using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

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
        DateTime Date)
        : IQuery<List<Response.YardPricesByDateDetailResponse>>;

    public record GetYardPricesByYardIdInTodayQuery(
        Guid UserId,
        Guid YardId)
        : IQuery<Response.YardPricesByDateDetailResponse>;
}
