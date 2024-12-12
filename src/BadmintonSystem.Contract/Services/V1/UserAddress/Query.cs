using BadmintonSystem.Contract.Abstractions.Message;
using BadmintonSystem.Contract.Abstractions.Shared;

namespace BadmintonSystem.Contract.Services.V1.UserAddress;

public static class Query
{
    public record GetUserAddressesQuery(Abstractions.Shared.Request.PagedQueryRequest Data)
        : IQuery<PagedResult<Response.UserAddressResponse>>;

    public record GetUserAddressByIdQuery(Guid Id)
        : IQuery<Response.UserAddressResponse>;
}
