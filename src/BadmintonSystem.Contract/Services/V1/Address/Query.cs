using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.Address;

public static class Query
{
    public record GetAddressesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.AddressResponse>>;

    public record GetAddressByIdQuery(Guid Id)
        : IQuery<Response.AddressResponse>;

    public record GetAddressesWithFilterAndSortValueQuery(
        Abstractions.Shared.Request.PagedFilterAndSortQueryRequest Data)
        : IQuery<PagedResult<Response.AddressDetailResponse>>;
}
