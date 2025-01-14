using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Provider;

public static class Query
{
    public record GetProviderByIdQuery(Guid Id)
        : IQuery<Response.ProviderDetailResponse>;

    public record GetProvidersWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.ProviderDetailResponse>>;
}
